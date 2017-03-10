using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {

    [HideInInspector]
    public int durability;
    public MapManager mapManager;
    public PlayerToWorldInteraction playerInteraction;
    //protected Transform heldItemPos;

    public enum E_ITEM
    {   
        STICK,     
        SHOVEL,
        STONEAXE,
        PICKAXE,
        DYNAMITE,
        TELEPORTER,
        OXYGEN_TANK,
        HEALTH_UP,
        SIZE                
    }

    public virtual void OnActionClick() {
        //durability--;
    }
    protected virtual void OnStart()
    {
        mapManager = FindObjectOfType<MapManager>();
        playerInteraction = FindObjectOfType<PlayerToWorldInteraction>();   
    }




    //******GETTING PREFAB FUNCTIONS
    private static Dictionary<E_ITEM, GameObject> itemPrefabs;

    public static GameObject GetPrefab(E_ITEM _id)
    {
        if (itemPrefabs == null)
            InitPrefabDictionary();

        GameObject returnItem = null;
        foreach (KeyValuePair<E_ITEM, GameObject> pair in itemPrefabs)
            if (pair.Key == _id)
                returnItem = pair.Value;

        if(returnItem == null)
            Debug.LogError("COULD NOT RETURN ITEM WITH ID = " + _id);

        return returnItem;                 
    }
    

    private static GameObject LoadPrefab(string _name)
    {
        return (GameObject)Resources.Load("Objects/Items/" + _name, typeof(GameObject));
    }
    private static void InitPrefabDictionary()
    {
        itemPrefabs = new Dictionary<E_ITEM, GameObject>();

        itemPrefabs.Add(E_ITEM.STICK, LoadPrefab("Stick"));
        itemPrefabs.Add(E_ITEM.SHOVEL, LoadPrefab("Shovel"));
        itemPrefabs.Add(E_ITEM.STONEAXE, LoadPrefab("Stoneaxe"));
        itemPrefabs.Add(E_ITEM.PICKAXE, LoadPrefab("Pickaxe"));
        itemPrefabs.Add(E_ITEM.DYNAMITE, LoadPrefab("Dynamite"));
        itemPrefabs.Add(E_ITEM.TELEPORTER, LoadPrefab("Teleporter"));
        itemPrefabs.Add(E_ITEM.OXYGEN_TANK, LoadPrefab("OxygenTank"));
        itemPrefabs.Add(E_ITEM.HEALTH_UP, LoadPrefab("HealthUp"));
    }
    

        
}
