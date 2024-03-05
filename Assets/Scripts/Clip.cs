using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Clip {

    public ClipData data;
    public string debugData;

    public float startTime = 0f;
    public float endTime = 0f;
    public float duration = 0f;

    public AudioClip audioClip;

    public IEnumerator DownloadData() {
        if (!string.IsNullOrEmpty(debugData)) {
            string txt = debugData;
            data = JsonConvert.DeserializeObject<ClipData>(txt);
        } else {
            string uri = $"https://oopaip.ina.fr/oopaip/api/python/next_job?login={User.main.user_login}";
            UnityWebRequest www = UnityWebRequest.Get(uri);
            var operation = www.SendWebRequest();
            while (!operation.isDone)
                yield return null;
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
                DisplayMessage.Instance.Display(www.error);
            } else {
                // Show results as text
                string txt = www.downloadHandler.text;
                data = JsonConvert.DeserializeObject<ClipData>(txt);
            }

        }
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator DownloadAudio() {
        // getting the start time of the audio clip

        Slot[] slots = data.GetSlots(ClipData.Field.Events);
        int startIndex = slots[0].stream.IndexOf('#') + 3;
        string timecode_start = slots[0].stream.Remove(0, startIndex);
        int lastIndex = timecode_start.LastIndexOf(',');
        timecode_start = timecode_start.Remove(lastIndex);
        startTime = SlotHandler.TimeCodeToTime(timecode_start);

        // getting the sent time of the audio clip
        string endStream = slots[slots.Length - 1].stream;
        string timecode_end = endStream.Remove(0, endStream.LastIndexOf(',') + 1);
        endTime = SlotHandler.TimeCodeToTime(timecode_end);

        // overarll duration
        duration = endTime - startTime;

        // audio clip path
        string path = slots[0].stream;
        int start = path.IndexOf("//");
        string auth = $"{User.main.user_login}:{User.main.user_password}@";
        path = path.Insert(start + 2, auth);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS)) {
            var op = www.SendWebRequest();
            while (!op.isDone) {
                DisplayLoading.Instance.SetJauge(www.downloadProgress);
                yield return null;
            }

            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log(www.error);
                DisplayMessage.Instance.Display(www.error);
            } else {
                AudioClip _clip = DownloadHandlerAudioClip.GetContent(www);
                audioClip = _clip;
            }

        }
    }

    public IEnumerator UploadData() {
        GameManager.Instance.HideGameUI();

        yield return new WaitForSeconds(1f);

        DisplayLoading.Instance.FadeIn();
        DisplayLoading.Instance.SetJauge(0f);

        yield return new WaitForSeconds(1f);

        string url = $"https://oopaip.ina.fr/oopaip/api/python/set_annotations?{User.main.GetAuth}";

        string json = JsonConvert.SerializeObject(data);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", jsonBytes);


        using (UnityWebRequest www = UnityWebRequest.Post(url, form)) {
            var operation = www.SendWebRequest();
            while (!operation.isDone) {
                DisplayLoading.Instance.SetJauge(www.uploadProgress);
                yield return null;
            }

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
                DisplayMessage.Instance.Display(www.error);
            } else {
                Debug.Log("Form upload complete!");
            }
        }

        yield return new WaitForSeconds(1f);

        DisplayLoading.Instance.FadeOut();
        DisplayStartLevel.Instance.FadeIn();
    }
}
