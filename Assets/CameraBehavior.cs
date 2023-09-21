using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraBehavior : MonoBehaviour
{
    public static CameraBehavior Instance;

    public int currentIndex = -1;
    public CameraButton[] CameraButtons;
    private Transform _transform;
    public Transform[] anchors;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetAnchor(1);
    }

    public void SetAnchor(int index)
    {
        if ( currentIndex >= 0)
        {
            CameraButtons[currentIndex].FadeIn();
        }
        currentIndex = index;
        CameraButtons[currentIndex].FadeOut();
        GetTransform.DOMove(anchors[index].position, 0.2f);
        GetTransform.DORotateQuaternion(anchors[index].rotation, 0.2f);
    }

    public Transform GetTransform
    {
        get
        {
            if ( _transform == null)
            {
                _transform = GetComponent<Transform>();
            }

            return _transform;
        }
    }
}
