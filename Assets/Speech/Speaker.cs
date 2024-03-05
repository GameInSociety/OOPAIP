using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class Speaker
{
    public int id;
    public int handlerIndex;
    public List<SlotHandler> handlers = new List<SlotHandler>();

    public SlotHandler CurrentHandler
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