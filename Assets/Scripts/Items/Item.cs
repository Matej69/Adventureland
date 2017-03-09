using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    [HideInInspector]
    public int durability;
    public MapManager mapManager;
    public PlayerToWorldInteraction playerInteraction;
    //protected Transform heldItemPos;

    public enum E_ITEM
    {        
        SHOVEL,
        STONEAXE,
        PICKAXE,
        DINAMITE,
        TELEPORTER,
        OXYGEN_TANK,
        SIZE                
    }

    public virtual void OnActionClick() {
        durability--;
    }
    protected virtual void OnStart()
    {
        mapManager = FindObjectOfType<MapManager>();
        playerInteraction = FindObjectOfType<PlayerToWorldInteraction>();

        Camera cam = Camera.main;
        transform.position = cam.transform.FindChild("HeldItemPos").transform.position;
        transform.parent = cam.transform;        
    }

        
}
