using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class Inventory : MonoBehaviour {

    Dictionary<n_block.E_BLOCK, int> blocks = new Dictionary<n_block.E_BLOCK, int>();
    Dictionary<Item.E_ITEM, int> items = new Dictionary<Item.E_ITEM, int>();

    public GameObject prefab_slot;

    public GameObject prefab_BlockInventory;
    GameObject ref_blockInventory;
    GameObject ref_itemInventory;

    // Use this for initialization
    void Start() {
        SetInitValuesForBlocks();
        SetInitValuesForItems();
    }

    // Update is called once per frame
    void Update() {

        HandelBlockInventoryGUI();
    }



    //******SET INIT VALUES******
    public void SetInitValuesForBlocks()
    {
        for (int i = 0; i < (int)n_block.E_BLOCK.SIZE; ++i)
            blocks.Add((n_block.E_BLOCK)i, 0);
    }

    public void SetInitValuesForItems()
    {
        for (int i = 0; i < (int)Item.E_ITEM.SIZE; ++i)
            items.Add((Item.E_ITEM)i, 0);
    }


    //******ADD AMOUNT OF UNITS******
    public void AddBlock(n_block.E_BLOCK _id)
    {
        for (int i = 0; i < (int)n_block.E_BLOCK.SIZE; ++i)
            if ((n_block.E_BLOCK)i == _id)
                blocks[(n_block.E_BLOCK)i] += 1;
    }


    //******FILL GUI*******
    public void FillBlockInfoToGUI()
    {
        GameObject ref_SlotsHolder = ref_blockInventory.transform.FindChild("Inventory").FindChild("ItemSlotsArea").gameObject;

        //Clear all slots before crating them and filling them
        for (int i = ref_SlotsHolder.transform.childCount - 1; i >= 0; --i)
            Destroy(ref_SlotsHolder.transform.GetChild(i).gameObject);

        //Create each slot and fill it with info
        foreach (KeyValuePair<n_block.E_BLOCK, int> pair in blocks)
        {
            if (pair.Value != 0)
            {
                //create new slot               
                GameObject newSlot = Instantiate(prefab_slot);
                newSlot.transform.SetParent(ref_SlotsHolder.transform);
                newSlot.transform.localScale = new Vector3(1, 1, 1);
                //add info to slot
                newSlot.transform.FindChild("Amount").GetComponent<Text>().text = pair.Value.ToString();
                newSlot.transform.FindChild("Image").GetComponent<Image>().sprite = n_block.BlockSprites.GetBlockSprite(pair.Key);
            }
        }
    }


    //******CREATE/DESTROY GUI******
    public void CreateBlockInventory()
    {
        ref_blockInventory = Instantiate(prefab_BlockInventory);
        FillBlockInfoToGUI();
    }
    public void DestroyBlockInventory()
    {
        Destroy(ref_blockInventory);
    }



    //****** CHECK FOR GUI STATE****
    public bool IsBlockGUIActive()
    {
        return (ref_blockInventory != null);
    }



    //********HANDLERS*******
    public void HandelBlockInventoryGUI()
    {
        //CREATE/HIDE ON BUTTON PRESS
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (IsBlockGUIActive())
                DestroyBlockInventory();
            else
                CreateBlockInventory();
        }
        //IF ACTIVE, UPDATE BLOCK STATUS
        if (ref_blockInventory != null)
            FillBlockInfoToGUI();
    }



}
