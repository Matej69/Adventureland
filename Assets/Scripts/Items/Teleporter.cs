using UnityEngine;
using System.Collections;

public class Teleporter : Item {
    

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
        playerStats.gameObject.GetComponent<PlayerToWorldInteraction>().ResetPosition();
        inventory.RemoveAmountOfItems(E_ITEM.TELEPORTER, 1);
        if (!inventory.IsItemInInventory(E_ITEM.TELEPORTER))
            inventory.DestroyItemInHand();
    }


}
