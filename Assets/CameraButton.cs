using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CameraButton : MonoBehaviour, IPointerClickHandler
{
    private Transform _transform;
    public int anchorID;
    CanvasGroup _canvasGroup;
    CanvasGroup CanvasGroup
    {
        get
        {
            if ( _canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            return _canvasGroup;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Tween.Bounce(GetTransform);

        CameraBehavior.Instance.SetAnchor(anchorID);
    }

    public void FadeIn()
    {
        CanvasGroup.DOFade(1f, 0.2f);
    }

    public void FadeOut()
    {
        CanvasGroup.DOFade(0.5f, 0.2f);
    }

    public Transform GetTransform { get { if (_transform == null) _transform = transform; return _transform; } }
}
