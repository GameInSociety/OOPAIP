using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabelButton : Displayable, IPointerClickHandler {
    public Label assignedLabel;
    public TextMeshProUGUI uiText;
    public Image image;

    public void Display (Label label) {

        FadeInInstant();
        assignedLabel = label;
        image.color = label.color;

        var name = assignedLabel.label;

        var dotIndex = name.IndexOf('.');
        if ( dotIndex >= 0 ) {
            name = name.Remove(0, dotIndex + 1);
        }

        uiText.text = name;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Tween.Bounce(GetTransform);

        if ( assignedLabel.children.Count > 0 ) {
            DisplayLabels.Instance.Display(DisplayLabels.Instance.currentField, assignedLabel.children, DisplayLabels.Instance.annotationId);
        } else {
            if (DisplayLabels.Instance.currentField == ClipData.Field.Events)
                NewEventLabel();
            else
                NewThematicLabel();
        }
    }

    public void NewThematicLabel() {
        var field = DisplayLabels.Instance.currentField;
        var fieldHandler = ClipPlayer.Instance.GetFieldHandler(field);
        var time = ClipPlayer.Instance.time;
        fieldHandler.AddHandler(assignedLabel.id);
        DisplayLabels.Instance.Close();
    }

    public void NewEventLabel() {
        var field = DisplayLabels.Instance.currentField;
        var fieldHandler = ClipPlayer.Instance.GetFieldHandler(field);

        var handler = fieldHandler.GetCurrentHandler();
        handler.SetAnnotation(DisplayLabels.Instance.annotationId, assignedLabel.id);

        fieldHandler.StartHandler(handler);

        DisplayLabels.Instance.Close();
    }


    public void ShowDescription() {
        DisplayMessage.Instance.Display(assignedLabel.description);

    }
}
