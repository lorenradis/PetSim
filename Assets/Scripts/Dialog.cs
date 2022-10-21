using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Dialog
{
    public string message;
    public string speakerName;
    public Sprite speakerPortrait;
    public string[] responses = new string[0];

    public bool isSpeech;


    public Dialog()
    {

    }

    public Dialog(string newMessage, string newName, Sprite newPortrait, bool _isSpeech, string response1, string response2, string response3, string response4)
    {
        message = newMessage;
        speakerName = newName;
        speakerPortrait = newPortrait;
        isSpeech = _isSpeech;

        responses = new string[4];

        if (response1 != "")
            responses[0] = response1;
        if (response2 != "")
            responses[1] = response2;
        if (response3 != "")
            responses[2] = response3;
        if (response4 != "")
            responses[3] = response4;
    }
}
