using UnityEngine;
using System.Collections;

public class BlockObject : MonoBehaviour {
    
    //[HideInInspector]
    public Vector3Int posID;
    public byte blockID;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Renderer>().sharedMaterial = n_block.BlockInfoDatabase.GetBlockInfo((n_block.E_BLOCK)blockID).material;
	}

    public void OnDestroyByPlayer()
    {
        //add block to inventory
        FindObjectOfType<Inventory>().AddBlock((n_block.E_BLOCK)blockID);
        //Create mini blocks with same material as block that was destroied
        GameObject destroiedBlock = (GameObject)(Resources.Load("Objects/Blocks/DestriedResource", typeof(GameObject)));
        Material mat = n_block.BlockInfoDatabase.GetBlockInfo((n_block.E_BLOCK)blockID).material;
        destroiedBlock.GetComponent<DestroiedBlock>().SetMaterial(mat);
        Instantiate(destroiedBlock, transform.position, Quaternion.identity);       
    }



}
