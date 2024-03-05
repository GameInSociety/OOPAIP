using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class TimelineSegment : MonoBehaviour {
    public Image image;
    public RectTransform rectTransform;

    public List<EventLabel> eventLabels = new List<EventLabel>();
    public EventLabel prefab;

    public SlotHandler handler;

    public void Init(SlotHandler _handler) {
        handler = _handler;
        UpdateUI();
        SetIdle();
    }

    private void Update() {
        if (handler.unfinished) {
            UpdateUI();
        }
    }

    public void SetActive() {

        var timeline = ClipPlayer.Instance.GetFieldHandler(handler.field).timeline;

        var c = Color.clear;
        if (handler.field == ClipData.Field.Events)
            c = timeline.idleColor;
        else
            c = handler.HasLabels() ? handler.GetLabel().color : Color.red;

        c = Color.Lerp(c, Color.black, 0.3f);
        image.DOColor(c, 0.2f);

        if ( handler.field  == ClipData.Field.Events) {

            for (int i = 0; i < handler.GetSlot.annotations.Count; i++) {
                if (i >= eventLabels.Count)
                    eventLabels.Add(Instantiate(prefab, timeline.parent));

                var annotation = handler.GetSlot.annotations[i];

                var context = handler.GetSlot.context_before[i];
                var timeStr = context.Remove(0, context.IndexOf('%') + 1);
                var time = SlotHandler.TimeCodeToTime(timeStr);

                var el = eventLabels[i];

                var halfTime = handler.StartTime + (handler.EndTime-handler.StartTime)/2f;

                if (!el.active) {
                    el.rectTransform.DOAnchorPos(new Vector2(GetPos(time), 20f), 0.2f);
                } else {
                    el.rectTransform.anchoredPosition = new Vector2(GetPos(time), 20f);
                    Tween.Bounce(el.GetTransform);
                }

                el.Show();
                el.SetLabel(handler.GetLabel(i));
            }
        }
    }

    public void SetIdle() {
        var timeline = ClipPlayer.Instance.GetFieldHandler(handler.field).timeline;
        if (handler.HasLabels() && handler.field != ClipData.Field.Events) {
            Color c = handler.GetLabel().color;
            image.color = c;
        } else
            image.color = timeline.idleColor;

        if (handler.field == ClipData.Field.Events) {

            int a = 0;
            foreach (var item in eventLabels) {
                if (a > 0)
                    item.Hide();
                ++a;
            }

            if (handler.GetSlot.annotations.Count == 0)
                return;

            if (eventLabels.Count == 0)
                eventLabels.Add(Instantiate(prefab, timeline.parent));

            var halfTime = handler.StartTime + (handler.EndTime - handler.StartTime) / 2f;
            eventLabels[0].rectTransform.DOAnchorPos(new Vector2(GetPos(halfTime), 20f), 0.2f);

            eventLabels[0].SetCount(handler.GetSlot.annotations.Count);
            eventLabels[0].Show();
        }
    }

    public void UpdateUI() {
        float w = GetPos(handler.EndTime) - GetPos(handler.StartTime);
        rectTransform.offsetMax = new Vector2(0f, 0f);
        rectTransform.offsetMin = new Vector2(0f, 0f);
        rectTransform.sizeDelta = new Vector2(w - 2, rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(GetPos(handler.StartTime), 0f);
    }

    float GetPos(float time) {
        float start = ClipPlayer.Instance.clip.startTime;
        float dur = ClipPlayer.Instance.clip.duration;
        float range = rectTransform.parent.GetComponent<RectTransform>().rect.width;
        float x = (time - start) / dur * range;
        return x;
    }
}
