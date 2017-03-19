using UnityEngine;
using System.Collections;

public class ShopOwner : MonoBehaviour {

    GameObject player;

    public GameObject prefab_Shop;
    public GameObject prefab_3DText;

    static GameObject ref_Shop;
    GameObject ref_3DText;

    MouseManager mouseManager;

    // Use this for initialization
    void Start () {        
        player = FindObjectOfType<PlayerStats>().gameObject;
        mouseManager = FindObjectOfType<MouseManager>().GetComponent<MouseManager>();

        CreateText();        

    }
	
	// Update is called once per frame
	void Update () {
        LookTowardsPlayer();

        HandleShopGUIExistance();
        Handle3DTextVisibility();
        
    }




    void LookTowardsPlayer()
    {
        transform.LookAt(player.transform);
        ref_3DText.transform.LookAt(player.transform);
    }


    void CreateShop()
    {
        ref_Shop = (GameObject)Instantiate(prefab_Shop);
        mouseManager.SetMouseSettings(MouseManager.E_MOUSE.ON_GUI);
    }
    void DestroyShop()
    {
        Destroy(ref_Shop);
        mouseManager.SetMouseSettings(MouseManager.E_MOUSE.IN_WORLD);
    }
    public static bool IsShopCreated()
    {
        return (ref_Shop != null);
    }


    void SetTextState(bool _state)
    {
        ref_3DText.SetActive(_state);      
    }
    void CreateText()
    {
        Vector3 pos = transform.position;
        pos.y += 3;
        ref_3DText = (GameObject)Instantiate(prefab_3DText, pos, Quaternion.identity);
    }


    void HandleShopGUIExistance()
    {     
        if (Vector3.Distance(transform.position, player.transform.position) < 7)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!IsShopCreated())
                    CreateShop();
                else
                    DestroyShop();
            }
        }
        else
        {
            DestroyShop();
        }
    }

    void Handle3DTextVisibility()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 7)
            SetTextState(true);        
        else
            SetTextState(false);        
    }


}
