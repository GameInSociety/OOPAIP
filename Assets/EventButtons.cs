using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButtons : MonoBehaviour
{
    public static EventButtons Instance;

    public EventButton[] buttons;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.Events).onSlotStart += HandleOnCurrentHandler;

        foreach (var button in buttons) {
            button.Hide();
        }

    }

    void HandleOnCurrentHandler(SlotHandler handler) {
        UpdateButtons(handler);
    }

    public void UpdateButtons(SlotHandler handler) {

        if ( handler == null) {
            return;
        }

        foreach (var button in buttons) {
            button.Hide();
        }

        for (int i = 0; i < buttons.Length; i++) {
            if (handler.GetSlot.annotations.Count >= (i+1)) {
                var id = handler.GetSlot.annotations[i];
                var label = Label.GetEventLabel(id);
                buttons[i].Display(label);
            }
        }

        int c = handler.GetSlot.annotations.Count;
        if (c >= 3)
            return;
        buttons[c].Display(null);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
