using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LabelLoader : MonoBehaviour
{
    public string path = "";

    private void Start()
    {
        StartCoroutine(LoadLabels());
    }

    IEnumerator LoadLabels()
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            string txt = www.downloadHandler.text;
            Label[] label = JsonConvert.DeserializeObject<Label[]>(txt);

            foreach (var item in label)
            {
                if (!item.label.Contains('.'))
                {
                    LabelGroup group = new LabelGroup();
                    group.name = item.label;
                    LabelGroup.NewGroup(group);

                }
                else
                {

                    Debug.Log("id : " + item.id);
                    Debug.Log("label : " + item.label);

                    LabelGroup lastGroup = LabelGroup.labelGroups[LabelGroup.labelGroups.Count - 1];
                    lastGroup.labels.Add(item);
                }
            }

        }
    }
}
