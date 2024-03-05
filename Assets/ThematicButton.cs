using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThematicButton : Displayable, IPointerClickHandler {

    public ClipData.Field field;
    public Label assignedLabel;

    public Image image;
    public TextMeshProUGUI uiText;

    public int annotationId;


    private void HandleOnSlotEnd(SlotHandler handler) {

    }

    void HandleOnEmptyHandler() {
        if (field == ClipData.Field.MainThematics)
            Display(null);
        else
            Hide();
    }

    private void HandleOnSlotStart(SlotHandler handler) {
        if ( handler.GetSlot.annotations.Count > 0) {

        }

    }

    public override void Hide() {
        base.Hide();
        Debug.Log($"hiding : {name}");
    }

    public void Display(Label label){
        FadeInInstant();
        assignedLabel = label;

        Tween.Bounce(GetTransform);

        if (label == null) {
            image.color = Color.white;
            uiText.text = field == ClipData.Field.MainThematics ? "Thématique\nPrincipale" : "Thématique\nSecondaire";
        } else {
            image.color = label.color;
            uiText.text = label.label;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        Tween.Bounce(GetTransform);

        DisplayMainUI.Instance.FadeOut();
        DisplayLabels.Instance.Display(field, Label.thematic_groups, annotationId);
    }

    public void AddThematicSlot(Label label) {

    }

}
