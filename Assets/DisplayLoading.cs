using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLoading : Displayable
{
    public static DisplayLoading Instance;

    private void Awake()
    {
        Instance = this;
    }
}
