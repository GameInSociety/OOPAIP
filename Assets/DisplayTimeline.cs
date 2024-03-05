using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTimeline : MonoBehaviour
{
    public TimelineSegment prefab;
    public List<TimelineSegment> segments = new List<TimelineSegment>();
    public Transform parent;

    public ClipData.Field field;

    RectTransform rectTransform;

    public Color idleColor;
    public Color currentColor;

    public float targetScale = 1f;
    public float scaleDuration = 1f;

    float startTime = 0f;
    float clipDuration = 0f;

    public float range;

    public void Init(float _startTime, float _duration) {
        startTime = _startTime;
        clipDuration = _duration;

        ClipPlayer.Instance.GetFieldHandler(field).onSlotStart += HandleOnSlotStart;
        ClipPlayer.Instance.GetFieldHandler(field).onSlotEnd += HandleOnSlotEnd;

        UpdateSegments(ClipPlayer.Instance.GetFieldHandler(field).handlers);
    }


    public void HandleOnSlotStart(SlotHandler slotHandler) {
        segments.Find(x => x.handler == slotHandler).SetActive();
    }


    public void HandleOnSlotEnd(SlotHandler slotHandler) {
        segments.Find(x => x.handler == slotHandler).SetIdle();
    }

    public void ZoomIn() {
        return;
        parent.DOScale(targetScale, scaleDuration);
        float lerp = (ClipPlayer.Instance.time - startTime) / clipDuration;
        float width = rectTransform.rect.width;
        float x = lerp * width;
        float w = (width / targetScale) / 2f;
        Vector2 pos = new Vector2(-x * targetScale + w, 0f);
        rectTransform.DOAnchorPos(pos, scaleDuration);
    }

    public void ZoomOut() {
        return;
        parent.DOScale(1f, scaleDuration);
        Vector2 p = rectTransform.anchoredPosition;
        p.x = 0f;
        rectTransform.DOAnchorPos(p, scaleDuration);
    }

    public void UpdateSegments(List<SlotHandler> slotHandlers)
    {
        rectTransform = GetComponent<RectTransform>();
        range = rectTransform.rect.width;

        for (int i = 0; i < slotHandlers.Count; i++) {
            var handler = slotHandlers[i];
            if ( i >= segments.Count)
                segments.Add(Instantiate(prefab, parent));
            segments[i].Init(handler);
        }
    }


    
}
