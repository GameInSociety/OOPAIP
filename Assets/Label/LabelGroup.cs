using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelGroup {
    public static List<LabelGroup> groups = new List<LabelGroup>();

    public LabelGroup (string id, string name) {
        this.id = id;
        this.name = name;
    }

    public string name;
    public string id;
    public List<Label> labels= new List<Label>();
}
