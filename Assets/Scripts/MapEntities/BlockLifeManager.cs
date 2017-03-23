using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Block will be take from list from behind, and inserted in list from front
//last object id is getting last created object ID <=> ID of array member that is not null
//--> [OBJ,OBJ,OBJ,->OBJ<-,NULL,NULL,NULL,NULL,NULL,NULL]

public class BlockLifeManager : MonoBehaviour {

    private static BlockLifeManager blockManager;

    public GameObject[] blockObjectList = new GameObject[1000];
    public int lastObjID = -1;
    public Vector3 blockBagPos = new Vector3(10, 10, 0);
    private GameObject prefab_blockObj;
    

    void Update()
    {
        FillWithObjects(10);       
    }




    public static BlockLifeManager Instance()
    {
        blockManager = (blockManager) ? blockManager : (blockManager = new GameObject("BLM").AddComponent<BlockLifeManager>());
        blockManager.prefab_blockObj = (blockManager.prefab_blockObj) ? blockManager.prefab_blockObj : (GameObject)Resources.Load("Objects/Blocks/BlockPrefab");
        return blockManager;
    }


    public GameObject TakeBlock()
    {       
        if (!IsListEmpty())
        {
            GameObject holdBlockObj = blockObjectList[lastObjID];
            blockObjectList[lastObjID] = null;
            lastObjID--;
            return holdBlockObj;           
        }        
        return null;                                 
    }

    public bool PutBlock(GameObject _blockObj)
    {        
        if (_blockObj != null && !IsListFull())
        {
            _blockObj.transform.position = blockBagPos;
            blockObjectList[lastObjID + 1] = _blockObj;
            lastObjID++;
            return true;
        }
        return false;
    }



    //Be carefull if you are using any kind of THREADS here, 
    //if value is bigger then list.length or less then -1, it won't work as expected
    public bool IsListFull()
    {
        return (lastObjID + 1 == blockObjectList.Length);
    }
    public bool IsListEmpty()
    {
        return (lastObjID == -1);
    }



    //Every frame if there is space in list, x number of new block GameObjects will be created
    public void FillWithObjects(int _blockNum)
    {
        for(int i = 0; i < _blockNum; ++i)
        {
            if (!IsListFull() && prefab_blockObj != null)
                PutBlock((GameObject)Instantiate(prefab_blockObj, blockBagPos, Quaternion.identity));
            else
                return;
        }
    }
    

}
