using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class FieldHandler
{
    public enum State {
        none,
        active,
        empty,
    }
    public State state;

    public string name;
    public ClipData.Field field;
    public List<SlotHandler> handlers = new List<SlotHandler>();
    public DisplayTimeline timeline;

    public delegate void OnSlotStart(SlotHandler handler);
    public OnSlotStart onSlotStart;
    public delegate void OnSlotEnd(SlotHandler handler);
    public OnSlotEnd onSlotEnd;
    public delegate void OnEmptyHandler();
    public OnEmptyHandler onEmptyHandler;

    private SlotHandler prevHandler;
    private SlotHandler currentHandler;
    public string handler_text = "";
    public bool triggeredEmptyHandler = false;

    public bool HasCurrentHandler() {
        return currentHandler != null && !string.IsNullOrEmpty(currentHandler.debug_name);
    }
    public SlotHandler GetHandler() {
        return currentHandler;
    }

    public void Init(Slot[] slots) {
        for (int i = 0; i < slots.Length; i++) {
            handlers.Add(new SlotHandler(slots[i], field));
        }

        timeline.Init(ClipPlayer.Instance.clip.startTime, ClipPlayer.Instance.clip.endTime);
    }

    public void AddHandler(string annotation) {

        var time = ClipPlayer.Instance.time;
        Debug.Log($"adding handler and slot at {time}, field : {field}");

        if (HasCurrentHandler()) {

            currentHandler.GetSlot.end = SlotHandler.FloatToTimeCode(time);
            EndHandler(currentHandler);
        }

        // new slot
        var newSlot = new Slot();
        newSlot.start = SlotHandler.FloatToTimeCode(time-0.1f);
        newSlot.end = SlotHandler.FloatToTimeCode(time+1);

        var sampleHandler = ClipPlayer.Instance.GetFieldHandler(ClipData.Field.Events).handlers[0];
        newSlot.stream = sampleHandler.GetSlot.stream;
        newSlot.media = sampleHandler.GetSlot.media;

        newSlot.context_before = new List<string>();
        newSlot.annotations = new List<string>();

        Debug.Log($"new slot start {newSlot.start}");
        Debug.Log($"new slot end {newSlot.end}");

        // end current handler

        var newHandler = new SlotHandler(newSlot, field);
        newHandler.debug_name += " (New)";
        newHandler.unfinished = true;

        if (!string.IsNullOrEmpty(annotation))
            newHandler.SetAnnotation(0, annotation);

        handlers.Add(newHandler);

        // fusion ici
        if ( field == ClipData.Field.MainThematics)
            FusionHandlers();


        // sort ici 
        handlers.Sort((x,y) => x.StartTime.CompareTo(y.StartTime));
       
        timeline.UpdateSegments(handlers);

        state = State.none;
        
    }

    void FusionHandlers() {
        for (int i = handlers.Count-1; i >= 1; i--) {
            var handler = handlers[i];
            var nextHandler = handlers[i - 1];

            if (handler.GetSlot.annotations.Count == 0 || nextHandler.GetSlot.annotations.Count == 0) {
                Debug.LogError($"Fusion Error : curr c {handler.GetSlot.annotations.Count} / next c {nextHandler.GetSlot.annotations.Count}");
                continue;
            }

            if (handler.GetSlot.annotations[0] == nextHandler.GetSlot.annotations[0]) {
                handler.GetSlot.start = nextHandler.GetSlot.start;
                handlers.RemoveAt(i-1);
                Debug.Log($"deleting : {i-1}");
            }
        }

    }

    public void Update(float time) {

        var newHandler = handlers.Find(x => time >= x.StartTime && time < x.EndTime);

        if ( newHandler == null) {
            if (state == State.empty)
                return;
            if ( state == State.active)
                EndHandler(currentHandler);
            if (onEmptyHandler != null)
                onEmptyHandler();
            state = State.empty;
        } else {
            if (state == State.active && currentHandler != newHandler) {
                EndHandler(currentHandler);
                currentHandler = newHandler;
                StartHandler(newHandler);
            }
            if ( state == State.none || state == State.empty) {
                state = State.active;
                currentHandler = newHandler;
                StartHandler(currentHandler);
            }
        }

        if ( currentHandler != null &&  currentHandler.unfinished) {
            var clampedTime = Mathf.Clamp(currentHandler.EndTime, time, 1000f);
            currentHandler.GetSlot.end = SlotHandler.FloatToTimeCode(time + 0.1f);
        }
    }


    public void StartHandler(SlotHandler slotHandler) {
        if (onSlotStart != null)
            onSlotStart(slotHandler);
        slotHandler.started = true;
    }
    public void EndCurrentHandler() {
        EndHandler(currentHandler);
    }
    public void EndHandler(SlotHandler slotHandler) {
        slotHandler.started = false;
        slotHandler.unfinished = false;

        if (onSlotEnd != null)
            onSlotEnd(slotHandler);
    }

    public SlotHandler GetCurrentHandler() {
        return currentHandler;
    }
}
