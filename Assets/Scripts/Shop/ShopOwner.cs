using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopOwner : MonoBehaviour {

    GameObject player;

    public GameObject prefab_Shop;
    public GameObject prefab_3DText;

    static GameObject ref_Shop;
    GameObject ref_3DText;

    MouseManager mouseManager;

    bool didShopOwnerSpoken = false;

    // Use this for initialization
    void Start () {        
        player = FindObjectOfType<PlayerStats>().gameObject;
        mouseManager = FindObjectOfType<MouseManager>().GetComponent<MouseManager>();

        CreateText();        

    }
	
	// Update is called once per frame
	void Update () {
        LookTowardsPlayer();

        HandlePlayerInteraction();
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


    void HandlePlayerInteraction()
    {     
        if (Vector3.Distance(transform.position, player.transform.position) < 7)
        {
            if (Input.GetKeyDown(KeyCode.E) && !Textbox.isTextboxActive)
            {
                if (!didShopOwnerSpoken)
                {
                    Textbox.GetInstance().EnableMessageBox(new List<TextboxMessageInfo> {
                        new TextboxMessageInfo("Me : Hey Frogman Freeman, what's up son?"),
                        new TextboxMessageInfo("Shop owner : What is hidden in the Earths core............."),
                        new TextboxMessageInfo("Shop owner : Is much more then single mortal can handle."),
                        new TextboxMessageInfo("Shop owner : You must be prepared for what awaits you on your adventure"),
                        new TextboxMessageInfo("Shop owner : Press 'I' to see what minerals you have acquired"),
                        new TextboxMessageInfo("Shop owner : ................."),
                        new TextboxMessageInfo("Shop owner : ................................."),
                        new TextboxMessageInfo("Shop owner : ................. BTW I'm #7 most popular meme in world so show me some respect...."),
                        new TextboxMessageInfo("Me : ................. ok, Green Boy Slim............ :-O "),
                        new TextboxMessageInfo("Shop owner : ;( ")
                    });
                    didShopOwnerSpoken = true;
                }
                else
                {
                    if (!IsShopCreated())
                        CreateShop();
                    else
                        DestroyShop();
                }
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
