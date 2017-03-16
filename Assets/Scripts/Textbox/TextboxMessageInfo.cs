using UnityEngine;
using System.Collections;

public class TextboxMessageInfo
{
    public int lastReadPos;
    public string msg;
    public string targetMsg;

    public TextboxMessageInfo(string _targetMsg)
    {
        targetMsg = _targetMsg;
        lastReadPos = 0;
        msg = "";
    }

    public bool IsMessageRead() { return (msg == targetMsg); }
    public void ReadLetter() { msg = msg + targetMsg[lastReadPos++]; }

}
