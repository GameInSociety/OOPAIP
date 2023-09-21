using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEditor;
using Newtonsoft.Json.Linq;
using System.Globalization;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class DownloadTest : MonoBehaviour
{
    public string path = "";
    public string soundURL = "";

    public AudioSource source;

    void Start()
    {
        //StartCoroutine(DownloadAudio());
        StartCoroutine(GetText());
    }
    IEnumerator DownloadAudio()
    {
        source = GetComponent<AudioSource>();
        using (var www = new WWW(soundURL))
        {
            yield return www;
            source.clip = www.GetAudioClip();
        }

        source.Play();
    }

    IEnumerator GetText()
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

            Debug.Log(txt);
            Speech[] speeches = JsonConvert.DeserializeObject<Speech[]>(txt);

            foreach (var item in speeches)
            {
                Debug.Log(item.speaker.Length);
            }

            //Speech[] speeches = JsonUtility.FromJson<Speech[]>(txt);

            /*JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.StringEscapeHandling = StringEscapeHandling.Default;
            settings.Formatting = Formatting.None;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Culture = CultureInfo.InvariantCulture;
            JsonConvert.DeserializeAnonymousType(txt, settings);
            Speech speech = JsonConvert.DeserializeObject<Speech>(txt, settings);
            Debug.Log(speech.media_id);*/
            
            /*string json = JsonUtility.ToJson(www.downloadHandler.text, true);
            Debug.Log(json);
            SpeechManager.instance.StartSpeech(json);*/

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}

public class Essai
{
    public string name;
    public string description;
}