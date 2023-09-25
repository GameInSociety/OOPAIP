using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator _animator;
    public Animator GetAnimator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }

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
