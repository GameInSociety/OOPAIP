using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLabels : Displayable {
    public ClipData.Field currentField;

    public static DisplayLabels Instance;

    public LabelButton prefab;
    public List<LabelButton> labelButtons;
    public ScrollRect scrollRect;

    public int annotationId = 0;


    public Transform parent;

    private void Awake() {
        Instance = this;
    }

    public void Display(ClipData.Field _field, List<Label> labels, int _targetId) {
        ClipPlayer.Instance.Pause();

        currentField = _field;

        annotationId = _targetId;

        FadeIn();
        scrollRect.verticalNormalizedPosition = 1f;
        foreach (var item in labelButtons)
            item.Hide();

        for (int i = 0; i < labels.Count; i++) {
            var label = labels[i];
            if (labelButtons.Count <= i) {
                var newButton = Instantiate(prefab, parent);
                labelButtons.Add(newButton);
            }
            labelButtons[i].Display(label);
        }
    }


    public void Close() {
        Instance.FadeOut();
        DisplayMainUI.Instance.FadeIn();
        ClipPlayer.Instance.Play();
    }
}
