using UnityEngine;
using System.Collections;

public class HealthUp : Item {
        
	// Use this for initialization
	void Awake () {
        OnStart();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public override void OnActionClick()
    {
        if (Textbox.isTextboxActive)
            return;
        SoundManager.GetInstance().PlaySound(SoundManager.E_NON_BLOCK_SOUND.HEALTH_UP);
        playerStats.ResetHealth();
        inventory.RemoveAmountOfItems(E_ITEM.HEALTH_UP, 1);
        if(!inventory.IsItemInInventory(E_ITEM.HEALTH_UP))
            inventory.DestroyItemInHand();    
    }


}
