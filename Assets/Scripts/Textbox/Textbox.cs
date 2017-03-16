using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Textbox : MonoBehaviour {

    private List<TextboxMessageInfo> messages = new List<TextboxMessageInfo>();

    Timer displayNewLetter;

    Text messageDisplayed;


	// Use this for initialization
	void Start () {
        messageDisplayed = transform.FindChild("Message").GetComponent<Text>();

        displayNewLetter = new Timer(0.02f);

        messages.Add(new TextboxMessageInfo("Yolo man read this yolo swag"));
        messages.Add(new TextboxMessageInfo("Second message I kill who do not obey"));
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
            else if (Input.GetKeyDown(KeyCode.Space))
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




    void EnableMessageBox(List<TextboxMessageInfo> _msgs)
    {
        messages = _msgs;
        GetComponent<Canvas>().enabled = true;
    }    

    void DisableMessageBox()
    {
        GetComponent<Canvas>().enabled = false;
    }





}
