using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public class DynamiteWorldObject : MonoBehaviour {

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
    void Start () {
        /*
        List<Vector3Int> positionIDs = new List<Vector3Int>() { new Vector3Int(1,1,1), new Vector3Int(1, 1, 1), new Vector3Int(1, 1, 1), new Vector3Int(1, 1, 1), new Vector3Int(1, 1, 2) };
        Debug.Log(positionIDs.Count);
        positionIDs = positionIDs.Distinct().ToList();
        Debug.Log(positionIDs.Count);
        */
        mapManager = FindObjectOfType<MapManager>();

        StartCoroutine(Explode(10f, null));       
    }
	
	// Update is called once per frame
	void Update () {
        //HandleRaycast();
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

    /*
    void HandleRaycast()
    {
        if(canMove)
        {
            float length = 0.2f;
            Vector3[] dirs = new Vector3[] { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            RaycastHit hitInfo;

            foreach (Vector3 dir in dirs)
            {
                bool didHit = Physics.Raycast(transform.position, dir, out hitInfo, length, m_block);
                if(didHit)
                {
                    canMove = false;
                    return;
                }
            }

        }


    }
    */


    void OnTriggerEnter(Collider col)
    {
        if (!touchingBlock && col.gameObject.tag == "BlockSolid")
        {
            canMove = false;
            touchingBlock = true;
            Vector3Int expPos = col.gameObject.GetComponent<BlockObject>().posID;
            StartCoroutine(Explode(2f, col.gameObject));            
        }
    }




    IEnumerator Explode(float _sec, GameObject _centerBlock)
    {
        yield return new WaitForSeconds(_sec);        
        Instantiate(prefab_Explosion, transform.position, Quaternion.identity);
        if (_centerBlock != null)
        {
            Vector3Int expPos = _centerBlock.GetComponent<BlockObject>().posID;
            DestroyBlocksInRange(_centerBlock);                  
        }
        Destroy(gameObject);
    }


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
                    x = (x < 0) ? 0 : ((x > n_chunk.CHUNK_SIZE.X - 1) ? n_chunk.CHUNK_SIZE.X - 1 : x);
                    y = (y < 0) ? 0 : ((y > n_chunk.CHUNK_SIZE.Y - 1) ? n_chunk.CHUNK_SIZE.Y - 1 : y);
                    z = (z < 0) ? 0 : ((z > n_chunk.CHUNK_SIZE.Z - 1) ? n_chunk.CHUNK_SIZE.Z - 1 : z);
                    positionIDs.Add(new Vector3Int(x, y, z));  
                }
        
        //get positions without bounds
        int boundOffset = 1;
        List<Vector3Int> withBoundIDs = new List<Vector3Int>();
        for (int x = centerPosID.x - halfRadius - boundOffset; x < centerPosID.x + halfRadius + boundOffset; ++x)
            for (int y = centerPosID.y - halfRadius - boundOffset; y < centerPosID.y + halfRadius + boundOffset; ++y)
                for (int z = centerPosID.z - halfRadius - boundOffset; z < centerPosID.z + halfRadius + boundOffset; ++z)
                {
                    x = (x < 0) ? 0 : ((x > n_chunk.CHUNK_SIZE.X - 1) ? n_chunk.CHUNK_SIZE.X - 1 : x);
                    y = (y < 0) ? 0 : ((y > n_chunk.CHUNK_SIZE.Y - 1) ? n_chunk.CHUNK_SIZE.Y - 1 : y);
                    z = (z < 0) ? 0 : ((z > n_chunk.CHUNK_SIZE.Z - 1) ? n_chunk.CHUNK_SIZE.Z - 1 : z);
                    withBoundIDs.Add(new Vector3Int(x, y, z));
                }

        //remove duplicate values (reason of duplication : bound limits)
        positionIDs = positionIDs.Distinct().ToList();
        withBoundIDs = withBoundIDs.Distinct().ToList();

        /*
        List<GameObject> blocksInRange = mapManager.GetBlocksObjectsInRange(_centerBlock, halfRadius);
        Debug.Log(blocksInRange.Count);
        blocksInRange = blocksInRange.Distinct().ToList();
        Debug.Log(blocksInRange.Count);
        */


        //clear from memory every block that is inside explosion radius, but not bounds
        List<BlockObject> blockObjInRange = mapManager.GetBlocksObjectsInRange(_centerBlock, halfRadius);
        foreach (Vector3Int pos in positionIDs)
        {
            mapManager.chunk.ClearBlockFromMemory(pos);
            for(int i = blockObjInRange.Count - 1; i >= 0; --i)
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
                if(!mapManager.chunk.blocks[posID.x, posID.z, posID.y].isVisible)
                    mapManager.CreateBlock(posID, (n_block.E_BLOCK)mapManager.chunk.GetBlock(posID).id);
            }
        }


    }

















}
