using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationButtons : MonoBehaviour
{
    public static NavigationButtons Instance;

    public Displayable left_Displayable;
    public Displayable right_Displayable;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.Events).onSlotStart += OnSetCurrentHandler;
    }

    private void OnSetCurrentHandler(SlotHandler slotHandler) {
        UpdateButtons();
    }

    public void UpdateButtons() {
        var fieldHandler = ClipPlayer.Instance.GetFieldHandler(ClipData.Field.Events);

        if (!fieldHandler.HasCurrentHandler())
            return;

        /*if (fieldHandler.GetIndex() < 1) {
            left_Displayable.Hide();
        } else {
            left_Displayable.Show();
        }

        if (fieldHandler.GetIndex() >= fieldHandler.handlers.Count -1) {
            right_Displayable.Hide();
        } else {
            right_Displayable.Show();
        }*/
    }
}
