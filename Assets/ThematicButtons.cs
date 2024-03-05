using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThematicButtons : MonoBehaviour {
    public static ThematicButtons Instance;

    public ThematicButton main;
    public ThematicButton secundary;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.MainThematics).onSlotStart += HandleOnSlotStart_Main;
        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.MainThematics).onEmptyHandler += HandleOnEmptyHandler_Main;
        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.SecondThematics).onSlotStart += HandleOnSlotStart_Sec;
        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.SecondThematics).onEmptyHandler += HandleOnEmptyHandler_Sec;
    }

    private void HandleOnSlotStart_Main(SlotHandler handler) {
        main.Display(handler.GetLabel());
        if (ClipPlayer.Instance.GetFieldHandler(ClipData.Field.SecondThematics).HasCurrentHandler()) {
            ClipPlayer.Instance.GetFieldHandler(ClipData.Field.SecondThematics).EndCurrentHandler();
        }
        secundary.Display(null);
    }

    private void HandleOnEmptyHandler_Main() {
        main.Display(null);
        secundary.Hide();
    }
    private void HandleOnSlotStart_Sec(SlotHandler handler) {

        if ( handler.GetSlot.annotations.Count == 0) {
            Debug.LogError($"probeleme");
            return;
        }

        secundary.Display(handler.GetLabel());

    }

    private void HandleOnEmptyHandler_Sec() {
        //secundary.Hide();
    }
}
