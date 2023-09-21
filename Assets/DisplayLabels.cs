using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLabels : Displayable
{
    public LabelButton prefab;
    public List<LabelButton> labelButtons;
    public ScrollRect scrollRect;

    public Transform parent;

    public static DisplayLabels Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Display(LabelGroup group)
    {
        scrollRect.verticalNormalizedPosition = 1f;
        FadeIn();
        Tween.Bounce(GetTransform);

        foreach (var item in labelButtons)
        {
            item.Hide();
        }

        for (int i = 0; i < group.labels.Count; i++)
        {
            Label label = group.labels[i];

            if ( labelButtons.Count <= i)
            {
                LabelButton newButton = Instantiate(prefab, parent);
                labelButtons.Add(newButton);
            }

            labelButtons[i].Display(label);
        }
    }
}
