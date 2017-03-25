using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



public class Inventory : MonoBehaviour {

    [HideInInspector]
    public Dictionary<n_block.E_BLOCK, int> blocks = new Dictionary<n_block.E_BLOCK, int>();
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
        
        PutItemInInventory(Tool.E_ITEM.PICKAXE, 1);        
        PutItemInInventory(Tool.E_ITEM.STICK, 1);
        PutItemInInventory(Tool.E_ITEM.AXE, 1);
        PutItemInInventory(Tool.E_ITEM.DYNAMITE, 1000);
        PutItemInInventory(Tool.E_ITEM.HEALTH_UP, 2);
        PutItemInInventory(Tool.E_ITEM.OXYGEN_TANK, 1);
        PutItemInInventory(Tool.E_ITEM.TELEPORTER, 1);
        PutItemInInventory(Tool.E_ITEM.SHOVEL, 1);

        FillItemSpritesToGUI();
    }

    // Update is called once per frame
    void Update() {
        HandleBlockInventoryGUI();

        HandleItemChange();
        FillItemInfoToGUI();

        HandleOnMouseClick();
    }



    //******SET INIT VALUES******
    private void SetInitValuesForBlocks()
    {
        for (int i = 0; i < (int)n_block.E_BLOCK.SIZE; ++i)
            blocks.Add((n_block.E_BLOCK)i, 0);
    }

    private void SetInitValuesForItems()
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


    //******FILL GUI******
    private void FillBlockInfoToGUI()
    {
        GameObject ref_SlotsHolder = ref_blockInventory.transform.FindChild("Inventory").FindChild("ItemSlotsArea").gameObject;

        //Clear all slots before creating them and filling them
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
                newSlot.transform.FindChild("Name").GetComponent<Text>().text = n_block.BlockSprites.GetBlockSprite(pair.Key).name;
                newSlot.transform.FindChild("Amount").GetComponent<Text>().text = pair.Value.ToString();
                newSlot.transform.FindChild("Image").GetComponent<Image>().sprite = n_block.BlockSprites.GetBlockSprite(pair.Key);
            }
        }
    }

    private void FillItemSpritesToGUI()
    {
        GameObject ref_holder = ref_itemInventory.transform.FindChild("ItemInventoryHolder").gameObject;
        int count = 0;
        foreach (Transform slot in ref_holder.transform)
        {
            //apply proper icon opacity to slot 
            Image img = slot.FindChild("Image").GetComponent<Image>();
            img.GetComponent<Image>().sprite = Item.GetInfo((Item.E_ITEM)count).iconSprite;

            ++count;
        }
    }

    private void FillItemInfoToGUI()
    {
        GameObject ref_holder = ref_itemInventory.transform.FindChild("ItemInventoryHolder").gameObject;

        int count = 0;
        foreach(Transform slot in ref_holder.transform)
        {
            //apply proper icon opacity to slot 
            Image img = slot.FindChild("Image").GetComponent<Image>();
            if (items[(Item.E_ITEM)count] == 0)
                img.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            else
                img.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            //apply border to selected slot
            Image border = slot.FindChild("SlotBorder").GetComponent<Image>();
            if (count == (int)itemInHandID)
                border.color = new Color(0, 1, 0, 1);
            else
                border.color = new Color(1, 1, 1, 1);
            //apply text for amount to slot
            bool doesTextExists = (slot.FindChild("Amount"));
            if (doesTextExists)
                slot.FindChild("Amount").GetComponent<Text>().text = items[(Item.E_ITEM)count].ToString();            

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

    private void CreateItemInventory()
    {
        ref_itemInventory = Instantiate(prefab_ItemInventory);
    }



    //****** CHECK FOR GUI STATE****
    private bool IsBlockGUIActive()
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
        if (ShopOwner.IsShopCreated())
            return;

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
        if (scrollDir != 0)
        {
            Destroy(ref_itemInHand);
            if (items[itemInHandID] != 0)
                PlaceItemInHand(itemInHandID);
        }

    }


    void HandleOnMouseClick()
    {
        if (ref_itemInHand != null)
        {
            Item itemRef = ref_itemInHand.GetComponent<Item>();
            //hold mouse down for tools
            if (Input.GetKey(KeyCode.Mouse0) && itemRef.usageType == Item.E_ITEM_USAGE.NON_STACKABLE)
                    ref_itemInHand.GetComponent<Item>().OnActionClick();
            //press mouse down for other items
            if (Input.GetKeyDown(KeyCode.Mouse0) && itemRef.usageType == Item.E_ITEM_USAGE.STACKABLE)
                    ref_itemInHand.GetComponent<Item>().OnActionClick();
        }
            
        

    }


    //*****ITEM OBJECT******

    void PlaceItemInHand(Tool.E_ITEM _id)
    {
        if (items[_id] != 0)
        {
            Vector3 itemPos = Camera.main.transform.FindChild("HeldItemPos").gameObject.transform.position;
            ref_itemInHand = (GameObject)Instantiate(Item.GetInfo(_id).prefab, itemPos, Quaternion.identity);
            ref_itemInHand.GetComponent<Item>().itemType = _id;
            ref_itemInHand.transform.parent = Camera.main.transform;
            itemInHandID = _id;
        }
        
    }

    public void DestroyItemInHand()
    {
        Destroy(ref_itemInHand);
    }



    //*****ITEM LOGIC******

    public void PutItemInInventory(Tool.E_ITEM _id, int _value)
    {
        items[_id] = _value;
    }

    public void RemoveAmountOfItems(Tool.E_ITEM _id, int _value)
    {
        items[_id] = (items[_id] - 1 < 0) ? 0 : items[_id] - 1;
    }

    public bool IsItemInInventory(Tool.E_ITEM _id)
    {
        foreach (KeyValuePair<Item.E_ITEM, int> item in items)
            if (item.Key == _id && item.Value > 0)
                return true;
        return false;
                 
    }


    public int GetBlockAmount(n_block.E_BLOCK _type)
    {
        foreach (KeyValuePair<n_block.E_BLOCK, int> pair in blocks)
            if (pair.Key == _type)
                return pair.Value;
        return 0;
    }



}
