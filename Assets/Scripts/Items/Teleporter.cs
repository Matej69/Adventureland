using UnityEngine;
using System.Collections;

public class Teleporter : Item {
    

    // Use this for initialization
    void Start()
    {
        OnStart();        
    }

    // Update is called once per frame
    void Update()
    {

    }
    



    public override void OnActionClick()
    {
        playerStats.gameObject.GetComponent<PlayerToWorldInteraction>().ResetPosition();
        inventory.RemoveAmountOfItems(E_ITEM.TELEPORTER, 1);
        if (!inventory.IsItemInInventory(E_ITEM.TELEPORTER))
            inventory.DestroyItemInHand();
    }


}
