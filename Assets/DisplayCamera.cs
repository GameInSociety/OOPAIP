using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCamera : Displayable
{
    public static DisplayCamera Instance;

    private void Awake()
    {
        Instance = this;
    }
}
