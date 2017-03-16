using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour {

    public enum E_MOUSE
    {
        ON_GUI,
        IN_WORLD
    }
    

    public Sprite noSprite;
    public Sprite handSprite;

    public Image mouseObj;


	// Use this for initialization
	void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        SpriteFollowsMouse();

    }


    public void SetMouseSprite(E_MOUSE _type)
    {
        mouseObj.sprite = (_type == E_MOUSE.ON_GUI) ? handSprite : noSprite;
    }

    private void SpriteFollowsMouse()
    {
        mouseObj.transform.position = Input.mousePosition;
    }


    public void SetMouseSettings(E_MOUSE _type)
    {                
        Cursor.lockState = (_type == E_MOUSE.IN_WORLD) ? CursorLockMode.Locked : CursorLockMode.None;
        SetMouseSprite(_type);        
    }







}
