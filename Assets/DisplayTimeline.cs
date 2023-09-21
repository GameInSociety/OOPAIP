using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DisplayTimeline : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;

    public RectTransform cursor;
    RectTransform rectTransform;
    public RectTransform jauge_RectTransform;

    private void Update()
    {
        if (SpeechManager.instance.playing && cursor != null)
        {
            float lerp = SpeechManager.instance.timer / SpeechManager.instance.duration;
            float x = lerp * rectTransform.rect.width;
            cursor.anchoredPosition = new Vector2(x, 0f);
            jauge_RectTransform.sizeDelta = new Vector2(x, rectTransform.rect.height);
        }
    }

    public void Init(float startTime, float endTime, float duration)
    {
        rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.rect.width;

        /*for (int i = 0; i < speaker.handlers.Count; i++)
        {
            SpeechHandler handler = speaker.handlers[i];
            GameObject go = Instantiate(prefab, parent);
            RectTransform rT = go.GetComponent<RectTransform>();
            float dur = (handler.EndTime - handler.StartTime);
            float x = handler.StartTime / lenght * width;
            float w = dur / lenght * width;
            rT.anchoredPosition = new Vector2(x, 0f);
            rT.sizeDelta = new Vector2( w, 8f );

        }*/
    }

}
