using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LabelButton : Displayable, IPointerClickHandler
{
    public TextMeshProUGUI uiText;
    public Label label;

    public void Display(Label _label)
    {
        FadeIn();
        label = _label;
        uiText.text = label.label;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Tween.Bounce(GetTransform);
        DisplayLabels.Instance.FadeOut();
        LabelizeButton.Instance.FadeIn();
        SpeechManager.instance.CurrHandler.AddAnnotation(label.id);

    }
}
