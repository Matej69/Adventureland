using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

    public GameObject blockPrefab;

    [HideInInspector]
    public n_chunk.Chunk chunk;

    // Use this for initialization
    void Start()
    {
        
        chunk = new n_chunk.Chunk();
        chunk.DoForEachBlock(InitialMapInstantiation);
        

    }

    
	// Update is called once per frame
	void Update () {
	
	}




    public GameObject InitialMapInstantiation(Vector3Int _posInt, n_block.E_BLOCK _type)
    {
        GameObject newBlockObj = null;
        if (_posInt.y == n_chunk.CHUNK_SIZE.Y - 1)
        {
            newBlockObj = CreateBlock(_posInt, _type);
            n_block.BlockLogic block = chunk.GetBlock(_posInt);
            block.isVisible = true;
        }
        return newBlockObj;
    }

    public GameObject CreateBlock(Vector3Int _posInt, n_block.E_BLOCK _type)
    {
        //change memory from null to new Block(...)
        n_block.BlockLogic midBlock = chunk.GetBlock(_posInt);
        midBlock = new n_block.BlockLogic(_posInt, (byte)_type);

        //create block object in scene
        GameObject newBlock = (GameObject)Instantiate(blockPrefab, new Vector3(_posInt.x, _posInt.y, _posInt.z), Quaternion.identity);
        newBlock.GetComponent<BlockObject>().posID = _posInt;
        newBlock.GetComponent<BlockObject>().blockID = (byte)_type;
        newBlock.transform.parent = transform;

        return newBlock;

    }


    public void DestroyBlock(GameObject _blockObj, n_chunk.Chunk _chunk)
    {
        //Destroy from memory
        Vector3Int posID = _blockObj.GetComponent<BlockObject>().posID;
        _chunk.ClearBlockFromMemory(posID);
        //Destroy from game scene
        _blockObj.GetComponent<BlockObject>().OnDestroyByPlayer();
        Destroy(_blockObj);
    }

    public void CreateSurroundingBlocks(n_chunk.Chunk _chunk, Vector3Int _pos)
    {
        //First check what is state of surrounding blocks in memory, and with that info decide if they need to be created in scene        
        Vector3Int[] aroundPos = new Vector3Int[]
        {
            new Vector3Int(_pos.x - 1,  _pos.y,         _pos.z),
            new Vector3Int(_pos.x + 1,  _pos.y,         _pos.z),
            new Vector3Int(_pos.x,      _pos.y - 1,     _pos.z),
            new Vector3Int(_pos.x,      _pos.y + 1,     _pos.z),
            new Vector3Int(_pos.x,      _pos.y,         _pos.z - 1),
            new Vector3Int(_pos.x,      _pos.y,         _pos.z + 1)
        };

        foreach(Vector3Int curPos in aroundPos)
        {
            if (_chunk.DoesBlockExists(curPos))
            {
                n_block.BlockLogic currBlock = chunk.GetBlock(curPos);
                if (!currBlock.isVisible)
                {
                    currBlock.isVisible = true;
                    CreateBlock(curPos, (n_block.E_BLOCK)currBlock.id);
                }        
            }           
        }   

    }













}
