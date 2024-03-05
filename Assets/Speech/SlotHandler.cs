using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static Unity.VisualScripting.AnnotationUtility;

[System.Serializable]
public class SlotHandler
{
    public string debug_name;
    public ClipData.Field field;

    public bool delete = false;

    [SerializeField]
    private Slot _slot;
    private float startTime;
    private float endTime;
    public bool started = false;
    bool ended = false;

    public bool unfinished = false;

    public SlotHandler(Slot slot, ClipData.Field field) {
        _slot = slot;
        this.field = field;
        debug_name = StartTime.ToString();
    }

    public void SetAnnotation(int id, string labelId) {
        var date = System.DateTime.Now;
        var time = SlotHandler.FloatToTimeCode(ClipPlayer.Instance.time);

        if (id >= _slot.annotations.Count) {
            _slot.annotations.Add("");
            _slot.context_before.Add("");
        }

        _slot.annotations[id] = labelId;
        _slot.context_before[id] = $"{date} % {time}";
    }
    public void AddAnnotation(string labelId) {

        var date = System.DateTime.Now;
        var time = SlotHandler.FloatToTimeCode(ClipPlayer.Instance.time);
        AddAnnotation(labelId, date.ToString(), time);
    }
    public void AddAnnotation(string annotation, string date, string time) {
        _slot.annotations.Add(annotation);
        _slot.context_before.Add($"{date} % {time}");
    }

    public bool HasLabels() {
        return GetSlot.annotations.Count > 0;
    }

    public Label GetLabel(int i = 0) {

        var id = GetSlot.annotations[i];
        if ( field == ClipData.Field.Events)
            return Label.GetEventLabel(id);
        else
            return Label.GetThematicLabel(id);
    }

    public int[] GetSpeakerIDs {
        get {
            int[] ids = new int[_slot.speaker.Length];

            for (int i = 0; i < _slot.speaker.Length; i++) {
                if (string.IsNullOrEmpty(_slot.speaker[i])){
                    ids[i] = 0;
                    continue;
                }

                string str = _slot.speaker[i].Remove(0, _slot.speaker[i].Length - 1);
                ids[i] = int.Parse(str);
            }

            return ids;
            
        }
    }


    public float StartTime {
        get {
            return TimeCodeToTime(_slot.start); ;
        }
    }

    public float EndTime
    {
        get
        {
            return TimeCodeToTime(_slot.end);
        }
    }

    public static float TimeCodeToTime(string str) {
        string seconds_Str = str.Remove(0, str.LastIndexOf(':') + 1);
        float seconds = float.Parse(seconds_Str, CultureInfo.InvariantCulture);
        string minutes_Str = str.Remove(0, str.IndexOf(':') + 1);
        minutes_Str = minutes_Str.Remove(minutes_Str.IndexOf(':'));
        float minutes = float.Parse(minutes_Str, CultureInfo.InvariantCulture);
        float f = (minutes * 60f) + seconds;
        return f;
    }

    public static string FloatToTimeCode(float time) {
        var ts = TimeSpan.FromSeconds((double)time);
        var str = ts.ToString();
        return str.Length >= 12 ? str.Remove(12) : str;
    }

    

    public Slot GetSlot {
        get { return _slot; }
    }
}
