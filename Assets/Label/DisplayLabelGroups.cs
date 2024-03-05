using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLabelGroups : Displayable
{
    public LabelGroupButton prefab;
    public List<LabelGroupButton> labelGroupButtons;
    public ScrollRect scrollRect;

    public Transform parent;

    public static DisplayLabelGroups Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Display(List<LabelGroup> labelGroups)
    {
        scrollRect.verticalNormalizedPosition = 1f;

        /*foreach (var item in labelGroupButtons)
        {
            item.Hide();
        }*/

        // pour l'instant, on prend que les 4 premiers
        //for (int i = 0; i < LabelGroup.labelGroups.Count; i++)
        for (int i = 0; i < labelGroups.Count; i++)
        {
            if (labelGroupButtons.Count <= i)
            {
                LabelGroupButton newButton = Instantiate(prefab, parent);
                labelGroupButtons.Add(newButton);
            }

            labelGroupButtons[i].Display(labelGroups[i]);
        }
    }
}
