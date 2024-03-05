using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Networking;

public class ClipLoader
{
    private static ClipLoader _instance;
    public static ClipLoader Instance {
        get {
            if (_instance == null )
                _instance = new ClipLoader();
            return _instance;
        }
    }
}
