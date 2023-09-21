using UnityEngine.EventSystems;

public class LabelizeButton : Displayable, IPointerClickHandler
{
    public static LabelizeButton Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();
        SpeechManager.instance.Resume();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DisplayLabelGroups.Instance.FadeInInstant();
        FadeOut();
        Tween.Bounce(GetTransform);
        SpeechManager.instance.Pause();
    }

}
