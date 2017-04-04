using UnityEngine;
using System.Collections;

public class OxygenTank : Item {
    
    void Awake()
    {
        OnStart();
    }

    // Use this for initialization
    void Start()
    {
        //OnStart();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void OnActionClick()
    {
        if (Textbox.isTextboxActive)
            return;
        playerStats.ResetOxygen();
        inventory.RemoveAmountOfItems(E_ITEM.OXYGEN_TANK, 1);
        if (!inventory.IsItemInInventory(E_ITEM.OXYGEN_TANK))
            inventory.DestroyItemInHand();
    }
}
