using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClipPlayer : MonoBehaviour {
    public static ClipPlayer Instance;

    public float time = 0f;

    public Clip clip;

    // handlers
    public FieldHandler[] fieldHandlers;

    // delegates
    public delegate void OnClipEnd();
    public OnClipEnd onClipEnd;
    public delegate void OnPlay();
    public OnPlay onPlay;
    public delegate void OnPause();
    public OnPause onPause;

    public bool playing = false;

    // audio
    public AudioSource audioSource = null;
    
    // pause
    public Displayable pause_displayable;

    // timelines
    public DisplayTimeline timeline_Events;
    public DisplayTimeline timeline_MainThematics;
    public DisplayTimeline timeline_SecundaryThematics;
    private void Awake() {
        Instance = this;
    }

    // Update is called once per frame
    void Update() {
      
        if (playing)
            UpdatePlayer();
    }

    public void InitPlayer(Clip clip) {
        this.clip = clip;
        audioSource.clip = clip.audioClip;
        audioSource.time = clip.startTime;
        time = clip.startTime;

        for (int i = 0; i < fieldHandlers.Length; ++i) {
            fieldHandlers[i].name = "" + (ClipData.Field)i;
            fieldHandlers[i].Init(clip.data.GetSlots((ClipData.Field)i));
            fieldHandlers[i].Update(time);
        }

        TimelineCursor.Instance.SetRange(timeline_Events.range);
        NavigationButtons.Instance.UpdateButtons();
    }

    public bool cursorControl = false;
    public void CursorControl_Start() {
        cursorControl = true;
    }

    public void CursorControl_End() {
        cursorControl = false;
    }

    void UpdatePlayer() {

        time += Time.deltaTime;

        if (time >= clip.endTime) {
            EndClip();
            return;
        }

        foreach (var item in fieldHandlers) {
            item.Update(time);
        }
    }


    public void Play() {
        timeline_Events.ZoomOut();

        playing = true;

        audioSource.time = time;
        audioSource.Play();
        pause_displayable.FadeOut();

        if (onPlay != null)
            onPlay();
    }

    public void Pause() {
        if (onPause != null)
            onPause();


        playing = false;
        audioSource.Pause();

        pause_displayable.FadeInInstant();
    }

    public void NextEvent() {
        var fieldHandler = GetFieldHandler(ClipData.Field.Events);

        var nextHandler = (SlotHandler)null;
        for (int i = 0; i < fieldHandler.handlers.Count; i++) {
            var handler = fieldHandler.handlers[i];
            if (handler.StartTime < time)
                continue;

            if (nextHandler != null) {
                if (time - nextHandler.StartTime < time - handler.StartTime)
                    nextHandler = handler;
            } else {
                nextHandler = handler;
            }

        }

        if ( nextHandler == null) {
            Debug.LogError($"found no handler at time : {time}");
        }


        time = nextHandler.StartTime;
        audioSource.time = nextHandler.StartTime;

        NavigationButtons.Instance.UpdateButtons();

    }
    public void PreviousEvent() {
        var fieldHandler = GetFieldHandler(ClipData.Field.Events);

        var closestHandler = (SlotHandler)null;
        for (int i = 0; i < fieldHandler.handlers.Count; i++) {
            var handler = fieldHandler.handlers[i];

            var dis = time - handler.EndTime;
            if (dis < 0)
                continue;

            if (closestHandler != null) {
                if (dis < time - closestHandler.EndTime )
                    closestHandler = handler;
            } else {
                closestHandler = handler;
            }

        }
        if ( closestHandler == null ) {
            time = 0f;
            audioSource.time = 0f;
            Debug.Log($"no prevous handler : resting");
        } else {
            time = closestHandler.StartTime;
            audioSource.time = closestHandler.StartTime;
        }
        
        NavigationButtons.Instance.UpdateButtons();
    }

    void EndClip() {
        Debug.Log($"clip ended");
        Pause();
        ClipManager.Instance.LoadNextClip();
    }

    public FieldHandler GetFieldHandler(ClipData.Field field) {
        return fieldHandlers[(int)field];
    }

}
