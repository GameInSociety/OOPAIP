using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventButton : Displayable, IPointerClickHandler
{
    public TextMeshProUGUI uiText;
    public Label assignedLabel;
    public Image image;
    public int id;

    public override void Start() {
        base.Start();
    }

    public void Display(Label _label) {
        FadeInInstant();

        Tween.Bounce(GetTransform);

        assignedLabel = _label;

        if ( _label == null) {
            image.color = Color.white;
            uiText.text = "Nouveau Label";
            return;
        }
        image.color = _label.color;
        uiText.text = assignedLabel.label;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Tween.Bounce(GetTransform);
        DisplayMainUI.Instance.FadeOut();
        DisplayLabels.Instance.Display(ClipData.Field.Events, Label.event_groups, id);

    }
    
}
