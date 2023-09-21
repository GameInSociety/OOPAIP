using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class Speaker
{
    public int id;
    public int handlerIndex;
    public List<SpeechHandler> handlers = new List<SpeechHandler>();

    public SpeechHandler CurrentHandler
    {
        get
        {
            return handlers[handlerIndex];
        }
    }
    public void NextHandler()
    {
        ++handlerIndex;
    }
}