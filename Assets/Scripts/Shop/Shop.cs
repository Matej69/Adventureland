using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour {

    public GameObject prefab_ItemSlot;

    public GameObject ref_ItemList;
    Inventory inventoryScr;

    List<GameObject> Slots = new List<GameObject>();    

	// Use this for initialization
	void Start () {
        inventoryScr = FindObjectOfType<Inventory>().GetComponent<Inventory>();

        InitItemSlots();
        SetButtonListeners();

    }
	
	// Update is called once per frame
	void Update () {
        FillSlotsWithInfos();

    }



    //Init slot for every item, except STICK
    void InitItemSlots()
    {
        for(int i = 1; i <= (int)Item.E_ITEM.SIZE - 1; ++i)
        {
            GameObject newSlot = (GameObject)Instantiate(prefab_ItemSlot, ref_ItemList.transform);
            newSlot.transform.localScale = new Vector3(1, 1, 0);   
            newSlot.transform.FindChild("ItemImage").GetComponent<Image>().sprite = Item.GetInfo((Item.E_ITEM)i).iconSprite;

            Slots.Add(newSlot);
        }
    }

    void FillSlotsWithInfos()
    {
        for (int i = 0; i <= (int)Item.E_ITEM.SIZE - 2; ++i)
        {
            //***SET REQUIREMENTS
            //Get requirement text components from GUI
            List<Text> requiremenText = new List<Text>();
            Transform Requirements = Slots[i].transform.FindChild("Requirements").transform;
            foreach (Transform child in Requirements)
                requiremenText.Add(child.gameObject.GetComponent<Text>());
            
            //Get requirement button component from GUI
            Button slotButton = Slots[i].transform.FindChild("Buy").GetComponent<Button>();
            Text slotButtonText = slotButton.transform.FindChild("Text").GetComponent<Text>();

            
            //***Apply info/visuals for TEXT to GUI for each block that item requires
            ItemInfo itemInfo = Item.GetInfo((Item.E_ITEM)i + 1);
            int reqTextIDToSetup = 0;
            bool allRequirementsFulfill = true;
            foreach (KeyValuePair<n_block.E_BLOCK, int> pair in itemInfo.requirements)
            {
                    //if we don't have required amount of one block, requirements are not fulfill
                    allRequirementsFulfill = (inventoryScr.GetBlockAmount(pair.Key) < pair.Value) ? false : allRequirementsFulfill;

                    Debug.Log(requiremenText.Count);
                    //Set text values
                    requiremenText[reqTextIDToSetup].text = n_block.BlockInfoDatabase.GetBlockInfo(pair.Key).name + " : " + inventoryScr.GetBlockAmount(pair.Key) + "/" + pair.Value;
                    requiremenText[reqTextIDToSetup].color = (inventoryScr.GetBlockAmount(pair.Key) >= pair.Value) ? new Color32(0, 255, 0, 255) : new Color32(255, 0, 0, 255);
                    reqTextIDToSetup++;                     
            }
            
            //***Apply info/visuals for BUTTON to GUI for each item slot
            Color btnColor = slotButton.image.color;
            Color textColor = slotButtonText.color;
            if (allRequirementsFulfill)
            {
                slotButton.image.color = new Color(btnColor.r, btnColor.g, btnColor.b, 1f);
                slotButtonText.color = new Color(textColor.r, textColor.g, textColor.b, 1f);
            }
            else
            {
                slotButton.image.color = new Color(btnColor.r, btnColor.g, btnColor.b, 0.3f);
                slotButtonText.color = new Color(textColor.r, textColor.g, textColor.b, 0.3f);
            }                          
        }


    }





    void SetButtonListeners()
    {
        for (int i = 0; i <= (int)Item.E_ITEM.SIZE - 2; ++i)
        {            
            Button slotButton = Slots[i].transform.FindChild("Buy").GetComponent<Button>();
            int fix_i = i + 1;
            slotButton.onClick.AddListener(() => OnButtonClick(fix_i));
        }
    }

    void OnButtonClick(int itemID)
    {
        //***Check if we have all requirements
        ItemInfo itemInfo = Item.GetInfo((Item.E_ITEM)itemID);
        bool allRequirementsFulfill = true;

        //check if we can buy it
        foreach (KeyValuePair<n_block.E_BLOCK, int> pair in itemInfo.requirements)
        {
            //if (itemInfo.DoesRequire(pair.Key))
            //{
                //if we don't have required amount of one block, requirements are not fulfill
                allRequirementsFulfill = (inventoryScr.GetBlockAmount(pair.Key) < pair.Value) ? false : allRequirementsFulfill;

            //}
        }
        //If we can buy it, buy item
        if (allRequirementsFulfill)
        {
            BuyItem((Item.E_ITEM)itemID);
        }
    }

    void BuyItem(Item.E_ITEM _itemID)
    {
        //place item in inventory
        inventoryScr.PutItemInInventory((Tool.E_ITEM)_itemID, 1);

        //reduce needed block resources from inventory
        ItemInfo itemInfo = Item.GetInfo((Item.E_ITEM)_itemID);
        foreach(KeyValuePair<n_block.E_BLOCK, int> pair in itemInfo.requirements)
        {
            inventoryScr.blocks[pair.Key] -= pair.Value;
        }   
    }







}
