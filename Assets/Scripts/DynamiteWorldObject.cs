using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class DynamiteWorldObject : MonoBehaviour
{

    public float explosionRadius;
    public float movementSpeed;
    public float gravity;

    public GameObject prefab_Explosion;

    public LayerMask m_block;

    float velY = 3;
    bool canMove = true;
    bool touchingBlock = false;

    MapManager mapManager;

    // Use this for initialization
    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }




    void HandleMovement()
    {
        if (canMove)
        {
            velY -= gravity * Time.deltaTime;
            transform.Translate(transform.forward * movementSpeed * Time.deltaTime, Space.World);
            transform.Translate(transform.up * velY * Time.deltaTime, Space.World);
        }
    }
    




    void OnTriggerEnter(Collider col)
    {
        if ( col.CompareTag("BlockSolid") && !touchingBlock)
        {
            canMove = false;
            touchingBlock = true;
            DestroyBlocksInRange(col.gameObject);
            Explode();
        }
        if (col.CompareTag("Zombie"))
        {
            Instantiate(prefab_Explosion, transform.position, Quaternion.identity);
            SoundManager.GetInstance().PlaySound(SoundManager.E_NON_BLOCK_SOUND.ZOMBIE_DEATH);
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
        if (col.CompareTag("Boss"))
        {
            Instantiate(prefab_Explosion, transform.position, Quaternion.identity);
            col.gameObject.GetComponent<Boss>().ReduceHealthBy(5f);
            Destroy(gameObject);
        }
    }
    
    void Explode()
    {
        Instantiate(prefab_Explosion, transform.position, Quaternion.identity);        
        Destroy(gameObject);
    }









    /*
    THIS NEEDS TO BE FASTER AND MUCH MORE LESS EXPENSIVE ON CPU
    */

    void DestroyBlocksInRange(GameObject _centerBlock)
    {
        int halfRadius = (int)explosionRadius / 2;

        Vector3Int centerPosID = _centerBlock.GetComponent<BlockObject>().posID;

        //get positions with bounds
        List<Vector3Int> positionIDs = new List<Vector3Int>();
        for (int x = centerPosID.x - halfRadius; x < centerPosID.x + halfRadius; ++x)
            for (int y = centerPosID.y - halfRadius; y < centerPosID.y + halfRadius; ++y)
                for (int z = centerPosID.z - halfRadius; z < centerPosID.z + halfRadius; ++z)
                {
                    if (x < 0 || x > n_chunk.CHUNK_SIZE.X - 1 || y < 0 || y > n_chunk.CHUNK_SIZE.Y - 1 || z < 0 || z > n_chunk.CHUNK_SIZE.Z - 1)
                        continue;
                    positionIDs.Add(new Vector3Int(x, y, z));
                }

        //get positions without bounds
        int boundOffset = 1;
        List<Vector3Int> withBoundIDs = new List<Vector3Int>();
        for (int x = centerPosID.x - halfRadius - boundOffset; x < centerPosID.x + halfRadius + boundOffset; ++x)
            for (int y = centerPosID.y - halfRadius - boundOffset; y < centerPosID.y + halfRadius + boundOffset; ++y)
                for (int z = centerPosID.z - halfRadius - boundOffset; z < centerPosID.z + halfRadius + boundOffset; ++z)
                {
                    if (x < 0 || x > n_chunk.CHUNK_SIZE.X - 1 || y < 0 || y > n_chunk.CHUNK_SIZE.Y - 1 || z < 0 || z > n_chunk.CHUNK_SIZE.Z - 1)
                        continue;
                    withBoundIDs.Add(new Vector3Int(x, y, z));
                }




        //clear from memory every block that is inside explosion radius, but not bounds
        List<BlockObject> blockObjInRange = mapManager.GetBlocksObjectsInRange(_centerBlock, halfRadius);
        foreach (Vector3Int pos in positionIDs)
        {
            mapManager.chunk.ClearBlockFromMemory(pos);
            for (int i = blockObjInRange.Count - 1; i >= 0; --i)
            {
                if (pos == blockObjInRange[i].posID)
                {
                    mapManager.DestroyBlock(blockObjInRange[i].gameObject, mapManager.chunk);
                }
            }
        }




        //spawn all blocks that are in area of explosion + bound blocks        
        foreach (Vector3Int posID in withBoundIDs)
        {
            if (mapManager.chunk.DoesBlockExists(posID))
            {
                if (!mapManager.chunk.blocks[posID.x, posID.z, posID.y].isVisible)
                    mapManager.CreateBlock(posID, (n_block.E_BLOCK)mapManager.chunk.GetBlock(posID).id);
            }
        }



    }



}