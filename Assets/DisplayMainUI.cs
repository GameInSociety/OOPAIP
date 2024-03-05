using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMainUI : Displayable {
    public static DisplayMainUI Instance;
    private void Awake() {
        Instance = this;
    }

    public override void Show() {
        base.Show();
    }
}
