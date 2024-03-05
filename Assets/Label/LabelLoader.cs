 using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class LabelLoader : MonoBehaviour {

    public Color[] thematics_Colors;
    public Color[] events_Colors;
    public Sprite[] thematics_Sprites;
    public Sprite[] events_Sprites;

    public const string eventPath = "https://oopaip.ina.fr/oopaip/api/python/discourse_values";
    public const string thematicPath = "https://oopaip.ina.fr/oopaip/api/python/thematic_values";
    List<Label> results = new List<Label>();

    public List<Label> debug_thematics;
    public List<Label> debug_events;

    private void Start()
    {
        StartCoroutine(LoadAllLabels());
    }

    IEnumerator LoadAllLabels() {

        yield return LoadLabels(thematicPath);
        Label.thematic_groups = InitLabels(results);
        Label.thematic_groups.RemoveRange(0, 9);
        debug_thematics = Label.thematic_groups;
        yield return LoadLabels(eventPath);
        Label.event_groups= InitLabels(results);
        Label.event_groups.RemoveRange(7, 2);
        Label.event_groups.RemoveRange(5, 1);
        debug_events = Label.event_groups;

        for (int i = 0; i < Label.thematic_groups.Count; i++) {
            var c = thematics_Colors[i];
            Label.thematic_groups[i].SetColor(c);
            Label.thematic_groups[i].sprite = thematics_Sprites[i];
            foreach (var label in Label.thematic_groups[i].children) {
                label.color = Color.Lerp(c, Color.white, 0.2f);
                label.sprite = thematics_Sprites[i];
            }
        }
        for (int i = 0; i < Label.event_groups.Count; i++) {
            var c = events_Colors[i];
            Label.event_groups[i].SetColor(events_Colors[i]);
            Label.event_groups[i].sprite = events_Sprites[i];
            foreach (var label in Label.event_groups[i].children) {
                label.color = Color.Lerp(c, Color.white, 0.2f);
                label.sprite = events_Sprites[i];
            }
        }
    }

    List<Label> InitLabels(List<Label> labels) {
        var groups = labels.FindAll(x => x.parents.Length == 0);
        labels.RemoveAll(x => x.parents.Length == 0);
        foreach (var lbl in labels) {
            foreach (var p in lbl.parents) {
                var group = groups.Find(x => p == x.id);
                if (group == null) {
                    Debug.Log($"no group found for parent id : {lbl.parents[0]}");
                } else {
                    group.AddChild(lbl);
                }
            }
        }
        return groups;
    }

    IEnumerator LoadLabels(string path)
    {
        UnityWebRequest www = UnityWebRequest.Get(path);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.LogError(www.error);
        } else {
            string txt = www.downloadHandler.text;
            results = JsonConvert.DeserializeObject<Label[]>(txt).ToList();
        }
    }
}
