using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelGroup
{
    public static List<LabelGroup> labelGroups = new List<LabelGroup>();
    public static void NewGroup(LabelGroup labelGroup)
    {
        labelGroups.Add(labelGroup);
    }
    public string name;
    public List<Label> labels= new List<Label>();
}
