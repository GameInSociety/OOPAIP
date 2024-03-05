using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Label
{
    public static List<Label> thematic_groups = new List<Label>();
    public static List<Label> event_groups = new List<Label>();

    public static Label GetEventLabel(string id) {
        var group = event_groups.Find(x => x.children.Find(x => x.id == id) != null);
        if ( group == null) {
            Debug.LogError($"EVENT : no group contains label with id : {id}");
            return null;
        }
        var label = group.children.Find(x=> x.id == id);
        if (label == null)
            Debug.LogError($"no EVENT label with id : {id}");
        return label;
    }
    public static Label GetThematicLabel(string id) {
        var group = thematic_groups.Find(x => x.children.Find(x => x.id == id) != null);
        if (group == null) {
            Debug.LogError($"THEMATIC : no group contains label with id : {id}");
            return null;
        }
        var label = group.children.Find(x => x.id == id);
        if (label == null)
            Debug.LogError($"no THEMATIC label with id : {id}");
        return label;
    }

    public string label;
    public string id;
    public string description;
    public string displayname;
    public string[] parents;
    public Color color;
    public Sprite sprite;
    public static bool IsNull(Label label) {
        return label == null || label != null && string.IsNullOrEmpty(label.id);
    }

    public List<Label> children = new List<Label>();
    public void AddChild(Label child) {
        children.Add(child);
    }

    public bool IsParent() {
        return children.Count > 0;
    }

    public void SetColor ( Color _color) {
        color = _color;
    }
    public Color GetColor() { return color; }
}
