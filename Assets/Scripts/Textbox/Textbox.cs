using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Textbox : MonoBehaviour {

    static public bool isTextboxActive = false;
    static Textbox instance;

    private List<TextboxMessageInfo> messages = new List<TextboxMessageInfo>();

    Timer displayNewLetter;

    Text messageDisplayed;
   
    static public Textbox GetInstance()
    {
        return ((instance != null) ? instance : instance = FindObjectOfType<Textbox>());
    }   

	// Use this for initialization
	void Start () {
        messageDisplayed = transform.FindChild("Message").GetComponent<Text>();

        displayNewLetter = new Timer(0.042f);
    }
	
	// Update is called once per frame
	void Update () {

        HandleMessageDisplay();

    }


    void HandleMessageDisplay()
    {
        displayNewLetter.Tick(Time.deltaTime);
        if(displayNewLetter.IsFinished() && AnyMessagesLeft())
        {
            if(!messages[0].IsMessageRead())
            {
                messages[0].ReadLetter();
                messageDisplayed.text = messages[0].msg;
            }      
            else if (Input.GetKey(KeyCode.Space))
            {
                messages.RemoveAt(0);
                if (!AnyMessagesLeft())
                    DisableMessageBox();
            }

            displayNewLetter.Reset();
        }
    }

    bool AnyMessagesLeft()
    {
        return (messages.Count != 0);
    }




    public void EnableMessageBox(List<TextboxMessageInfo> _msgs)
    {
        messages = _msgs;
        GetComponent<Canvas>().enabled = true;
        Textbox.isTextboxActive = true;
    }    

    void DisableMessageBox()
    {
        GetComponent<Canvas>().enabled = false;
        Textbox.isTextboxActive = false;
    }





}
