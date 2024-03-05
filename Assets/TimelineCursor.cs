using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TimelineCursor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public static TimelineCursor Instance;
    RectTransform rectTranform;
    public float range;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        rectTranform = GetComponent<RectTransform>();
    }

    public void SetRange(float range) {
        this.range = range;
    }

    // Update is called once per frame
    void Update()
    {

        if (ClipPlayer.Instance.playing ) {
            Clip clip = ClipPlayer.Instance.clip;
            float lerp = (ClipPlayer.Instance.time - clip.startTime) / clip.duration;
            float x = lerp * range;
            rectTranform.anchoredPosition = new Vector2(x, rectTranform.anchoredPosition.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        ClipPlayer.Instance.CursorControl_Start();
    }

    public void OnPointerUp(PointerEventData eventData) {
        ClipPlayer.Instance.CursorControl_End();
    }
}
