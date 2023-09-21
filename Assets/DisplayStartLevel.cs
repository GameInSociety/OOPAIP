using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStartLevel : Displayable
{
    public static DisplayStartLevel Instance;
    private void Awake()
    {
        Instance = this;
    }
}
