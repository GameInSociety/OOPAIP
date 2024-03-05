using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLoading : Displayable {
    public static DisplayLoading Instance;

    public Image fill_image;

    private void Awake() {
        Instance = this;
    }

    public override void Show() {
        base.Show();
        SetJauge(0f);
    }

    public void SetJauge(float value) {
        fill_image.fillAmount = (value+0.03f);
    }
}
