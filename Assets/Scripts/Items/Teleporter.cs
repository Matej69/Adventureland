using UnityEngine;
using System.Collections;

public class Teleporter : Item {

    Vector3 teleportToPos;

    // Use this for initialization
    void Start()
    {
        OnStart();
        teleportToPos = GameObject.Find("TeleporterFreeObject").gameObject.transform.position;
        teleportToPos.y += 0.3f;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void OnActionClick()
    {
        playerStats.gameObject.transform.position = teleportToPos;
        inventory.RemoveAmountOfItems(E_ITEM.TELEPORTER, 1);
        if (!inventory.IsItemInInventory(E_ITEM.TELEPORTER))
            inventory.DestroyItemInHand();
    }


}
