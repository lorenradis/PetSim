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
    private string v1;
    private string v2;
    private string v3;
    private string v4;
    private string v5;
    private Action p1;
    private Action p2;

    public Dialog()
    {

    }

    public Dialog(string v1, string v2, string v3, string v4, string v5, Action p1, Action p2)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.v4 = v4;
        this.v5 = v5;
        this.p1 = p1;
        this.p2 = p2;
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
