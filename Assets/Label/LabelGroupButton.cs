using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LabelGroupButton : Displayable, IPointerClickHandler
{
    public TextMeshProUGUI uiText;

    LabelGroup labelGroup;

    public void Display(LabelGroup _labelGroup)
    {
        this.labelGroup = _labelGroup;
        uiText.text = _labelGroup.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Tween.Bounce(GetTransform);
        DisplayLabelGroups.Instance.FadeOut();
    }
}