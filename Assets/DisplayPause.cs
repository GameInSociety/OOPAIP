using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPause : Displayable
{
    public static DisplayPause Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();

        Tween.Bounce(GetTransform);
    }
}
