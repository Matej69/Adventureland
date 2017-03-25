using UnityEngine;
using System.Collections;

public class BlockObject : MonoBehaviour {
        
    [HideInInspector]
    public Vector3Int posID;
    [HideInInspector]
    public byte blockID;
    [HideInInspector]
    public byte durability;
    
	// Use this for initialization
	void Start ()
    {            
	}




    private void SetDurability(byte _amount)
    {
        durability = _amount;
    }
    private void ReduceDurability(byte _amount)
    {
        _amount = (durability < _amount) ? durability : _amount; 
        durability -= _amount;
    }

    public void SetProperties(Vector3Int _posID, byte _blockID, byte _durability, Material _material)
    {
        posID = _posID;
        blockID = _blockID;
        durability = _durability;
        GetComponent<Renderer>().sharedMaterial = _material;
        transform.position = new Vector3(_posID.x, -_posID.y, _posID.z);
    }










    public void OnDestroyByPlayer()
    {
        //add block to inventory
        FindObjectOfType<Inventory>().AddBlock((n_block.E_BLOCK)blockID);
        //spawn 3DText of block that was destroied
        GameObject blockText = (GameObject)Instantiate(n_block.BlockText.GetPrefab(), transform.position, Quaternion.identity);
        blockText.GetComponent<BlockPickupText>().SetText(((n_block.E_BLOCK)blockID).ToString());
        //Create mini blocks with same material as block that was destroied
        GameObject destroiedBlock = (GameObject)(Resources.Load("Objects/Blocks/DestriedResource", typeof(GameObject)));
        Material mat = n_block.BlockInfoDatabase.GetBlockInfo((n_block.E_BLOCK)blockID).material;
        destroiedBlock.GetComponent<DestroiedBlock>().SetMaterial(mat);
        Instantiate(destroiedBlock, transform.position, Quaternion.identity);
        //Destroy block and create sorounding blocks
        MapManager mapManager = FindObjectOfType<MapManager>();
        mapManager.CreateSurroundingBlocks(mapManager.chunk, posID);
        mapManager.DestroyBlock(gameObject, mapManager.chunk);
    }
       

    public void OnToolHit(byte _damageAmount)
    {        
        ReduceDurability(_damageAmount);
        if (durability <= 0)
            OnDestroyByPlayer();
    }

    
    



}
