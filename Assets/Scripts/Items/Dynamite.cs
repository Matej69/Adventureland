using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dynamite : Item {
    
    public GameObject prefab_dynamiteWorldObj;    

    // Use this for initialization
    void Awake()
    {
        OnStart();        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void OnActionClick()
    {
        if (Textbox.isTextboxActive)
            return;
        CreateFreeDynamiteObj();
        inventory.RemoveAmountOfItems(E_ITEM.DYNAMITE, 1);
        if (!inventory.IsItemInInventory(E_ITEM.DYNAMITE))
            inventory.DestroyItemInHand();
    }

    void CreateFreeDynamiteObj()
    {
        GameObject dynamite = (GameObject)Instantiate(prefab_dynamiteWorldObj, transform.position, Quaternion.identity);
        dynamite.GetComponent<DynamiteWorldObject>().transform.rotation = playerStats.transform.rotation;
    }


}
