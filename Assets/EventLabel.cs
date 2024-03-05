using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventLabel : Displayable {

    public bool active = false;
    public RectTransform rectTransform;
    public Image bg;
    public Image icon;
    public TextMeshProUGUI uiText;
    public GameObject iconGroup;
    public GameObject textGroup;

    public override void Show() {
        base.Show();
        Debug.Log($"show");
    }

    public override void Hide() {
        base.Hide();
        Debug.Log($"hide");
    }

    public void SetLabel(Label label) {
        active = true;

        textGroup.SetActive(false);

        bg.color = label.color;
        iconGroup.SetActive(true);
        icon.sprite = label.sprite;
    }

    public void SetCount(int count) {
        active = false;

        iconGroup.SetActive(false);

        bg.color = Color.white;
        textGroup.SetActive(true);
        uiText.text = count.ToString();
    }

}
