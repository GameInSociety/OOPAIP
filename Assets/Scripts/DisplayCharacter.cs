using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DisplayCharacter : MonoBehaviour
{
    public int id;

    private Animator _animator;
    public Animator GetAnimator {
        get {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();
            return _animator;
        }
    }

    public FaceControl _faceControl;

    public GameObject talkingFeedback;

    bool talking = false;

    private void Start()
    {
        _faceControl = GetComponentInChildren<FaceControl>();
        StopTalking();

        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.Events).onSlotStart += HandleOnCurrentHandler;
        ClipPlayer.Instance.GetFieldHandler(ClipData.Field.Events).onSlotEnd += HandleOnEndHandler;
        ClipPlayer.Instance.onPause += OnPause;

    }

    private void HandleOnCurrentHandler(SlotHandler handler) {
        UpdateCharacter(handler);
    }

    private void HandleOnEndHandler(SlotHandler handler) {
        StopTalking();
    }

    void OnPause() {
        StopTalking();
    }

    void UpdateCharacter(SlotHandler slotHandler) {
        if (slotHandler == null)
            return;
        StopTalking();
        foreach (var i in slotHandler.GetSpeakerIDs) {
            if ( i == this.id)
                StartTalking();
        }
    }

    public void StartTalking()
    {
        talking = true;
        GetAnimator.enabled = true;
        GetAnimator.SetBool("talking", true);
        _faceControl.StartAnim();
        talkingFeedback.SetActive(true);
        Tween.Bounce(talkingFeedback.transform);
    }

    public void StopTalking()
    {
        talking = false;
        GetAnimator.SetBool("talking", false);
        _faceControl.Stop();
        talkingFeedback.SetActive(false);
    }
}
