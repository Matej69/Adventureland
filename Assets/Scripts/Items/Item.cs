﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemInfo
{
    public string name;
    public GameObject prefab;
    public Sprite iconSprite;
    public Dictionary<n_block.E_BLOCK, int> requirements;

    //public bool DoesRequire(n_block.E_BLOCK _id) { return (requirements.ContainsKey(_id) == true);  }
}

public class Item : MonoBehaviour {

    [HideInInspector]
    public int durability;
    [HideInInspector]
    public MapManager mapManager;
    [HideInInspector]
    public PlayerToWorldInteraction playerInteraction;

    
    public enum E_ITEM
    {   
        STICK,     
        SHOVEL,
        AXE,
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




    //******GETTING OBJECT PREFAB OR ICON FUNCTIONS
    private static Dictionary<E_ITEM, ItemInfo> itemInfo;

    public static ItemInfo GetInfo(E_ITEM _id)
    {
        if (itemInfo == null)
            InitInfoDictionary();

        ItemInfo returnItem = null;
        foreach (KeyValuePair<E_ITEM, ItemInfo> pair in itemInfo)
            if (pair.Key == _id)
                returnItem = pair.Value;

        if(returnItem == null)
            Debug.LogError("COULD NOT RETURN ITEM WITH ID = " + _id);

        return returnItem;                 
    }
    

    private static ItemInfo LoadInfo(string _name, Dictionary<n_block.E_BLOCK, int> _requirements)
    {
        ItemInfo info = new ItemInfo();
        info.name = _name;
        info.prefab = (GameObject)Resources.Load("Objects/Items/" + _name, typeof(GameObject));
        info.iconSprite = (Sprite)Resources.Load("Sprites/ItemIcons/" + _name, typeof(Sprite));
        info.requirements = _requirements;
        return info;
    }
    private static void InitInfoDictionary()
    {
        itemInfo = new Dictionary<E_ITEM, ItemInfo>();

        itemInfo.Add(E_ITEM.STICK,          LoadInfo("Stick",       new Dictionary<n_block.E_BLOCK, int> { }));
        itemInfo.Add(E_ITEM.SHOVEL,         LoadInfo("Shovel",      new Dictionary<n_block.E_BLOCK, int> { { n_block.E_BLOCK.IRON, 1 }, { n_block.E_BLOCK.STONE, 0 } } ));
        itemInfo.Add(E_ITEM.AXE,            LoadInfo("Axe",         new Dictionary<n_block.E_BLOCK, int> { { n_block.E_BLOCK.METAL, 20 - 19 }, { n_block.E_BLOCK.STONE, 40 - 39 }, { n_block.E_BLOCK.IRON, 40 - 39 } } ));
        itemInfo.Add(E_ITEM.PICKAXE,        LoadInfo("Pickaxe",     new Dictionary<n_block.E_BLOCK, int> { { n_block.E_BLOCK.METAL, 40 }, { n_block.E_BLOCK.IRON, 60 }, { n_block.E_BLOCK.GOLD, 40 } }));
        itemInfo.Add(E_ITEM.DYNAMITE,       LoadInfo("Dynamite",    new Dictionary<n_block.E_BLOCK, int> { { n_block.E_BLOCK.COAL, 30 }, { n_block.E_BLOCK.LAPIS, 3 }, { n_block.E_BLOCK.SAND, 20 } }));
        itemInfo.Add(E_ITEM.TELEPORTER,     LoadInfo("Teleporter",  new Dictionary<n_block.E_BLOCK, int> { { n_block.E_BLOCK.LAPIS, 10 }, { n_block.E_BLOCK.DIAMOND, 10 }, { n_block.E_BLOCK.EMERALD, 2 } }));
        itemInfo.Add(E_ITEM.OXYGEN_TANK,    LoadInfo("OxygenTank",  new Dictionary<n_block.E_BLOCK, int> { { n_block.E_BLOCK.RUBY, 4 }, { n_block.E_BLOCK.EMERALD, 2 }, { n_block.E_BLOCK.IRON, 50 } }));
        itemInfo.Add(E_ITEM.HEALTH_UP,      LoadInfo("HealthUp",    new Dictionary<n_block.E_BLOCK, int> { { n_block.E_BLOCK.RUBY, 25 }, { n_block.E_BLOCK.DIAMOND, 20 }, { n_block.E_BLOCK.EMERALD, 5 } }));
    }
    

        
}
