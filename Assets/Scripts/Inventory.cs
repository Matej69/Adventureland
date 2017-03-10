using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class Inventory : MonoBehaviour {

    Dictionary<n_block.E_BLOCK, int> blocks = new Dictionary<n_block.E_BLOCK, int>();
    Dictionary<Item.E_ITEM, int> items = new Dictionary<Item.E_ITEM, int>();

    Item.E_ITEM itemInHandID = Item.E_ITEM.STICK;
    GameObject ref_itemInHand;

    public GameObject prefab_slot;

    public GameObject prefab_BlockInventory;
    public GameObject prefab_ItemInventory;
    GameObject ref_blockInventory;
    GameObject ref_itemInventory;

    // Use this for initialization
    void Start() {
        SetInitValuesForBlocks();
        SetInitValuesForItems();

        CreateItemInventory();

        PlaceItemInHand(Tool.E_ITEM.PICKAXE);

    }

    // Update is called once per frame
    void Update() {
        HandleBlockInventoryGUI();

        HandleItemChange();
        FillItemInfoToGUI();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(ref_itemInHand != null)
                ref_itemInHand.GetComponent<Item>().OnActionClick();
        }
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

    public void FillItemInfoToGUI()
    {
        GameObject ref_holder = ref_itemInventory.transform.FindChild("ItemInventoryHolder").gameObject;

        int count = 0;
        foreach(Transform slot in ref_holder.transform)
        {
            Image img = slot.FindChild("Image").GetComponent<Image>();
            Image border = slot.FindChild("SlotBorder").GetComponent<Image>();

            

            //if we are on a slot where are item should be, select that slot(change border color)
            if (count == (int)itemInHandID)
                border.color = new Color(0, 1, 0, 1);
            else
                border.color = new Color(1, 1, 1, 1);

            count++;
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

    public void CreateItemInventory()
    {
        ref_itemInventory = Instantiate(prefab_ItemInventory);
    }



    //****** CHECK FOR GUI STATE****
    public bool IsBlockGUIActive()
    {
        return (ref_blockInventory != null);
    }

       

    //********HANDLERS*******
    public void HandleBlockInventoryGUI()
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

    //******* SWITCH ITEM IN HAND
    void HandleItemChange()
    {
        //change item-inventory visuals
        float scrollDir = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDir > 0)
        {
            itemInHandID = (Item.E_ITEM)(((int)itemInHandID + 1) % (int)Item.E_ITEM.SIZE);
        }
        if (scrollDir < 0)
        {
            if ((int)itemInHandID == 0)
                itemInHandID = Item.E_ITEM.SIZE - 1;
            else
                itemInHandID = (Item.E_ITEM)(((int)itemInHandID - 1) % (int)Item.E_ITEM.SIZE);
        }
        //create tool an place it in hand
        if(scrollDir != 0)
            PlaceItemInHand(itemInHandID);


    }

    void PlaceItemInHand(Tool.E_ITEM _id)
    {
        if(ref_itemInHand != null)
            Destroy(ref_itemInHand);

        Debug.Log("sss");
        Vector3 itemPos = Camera.main.transform.FindChild("HeldItemPos").gameObject.transform.position;
        ref_itemInHand = (GameObject)Instantiate(Item.GetPrefab(_id), itemPos, Quaternion.identity);
        ref_itemInHand.transform.parent = Camera.main.transform;
        itemInHandID = _id;
        
    }




}
