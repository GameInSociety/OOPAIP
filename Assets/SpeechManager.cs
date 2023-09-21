using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using UnityEditorInternal;

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager instance;

    public string tmp_url = "";

    public Character[] characters = null;

    public Speech[] speeches;
    //public Speaker[] speakers = new Speaker[2];
    public SpeechHandler[] handlers;
    public DisplayTimeline displayTimeline;

    public float timer = 0f;
    public float startTime = 0f;
    public float endTime = 0f;
    public float duration = 0f;

    public bool playing = false;
    public int handlerIndex = 0;

    public AudioSource audioSource = null;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if ( playing)
            UpdateSpeeches();
    }

    void UpdateSpeeches()
    {
        timer += Time.deltaTime;

        if ( Input.GetKeyDown(KeyCode.T))
            timer = duration -2f;

        if ( timer >= duration)
            EndSpeeches();

        if ( handlerIndex >= handlers.Length)
            return;

        if (CurrHandler.Started(timer))
        {
            Debug.Log("new speech");
            foreach (var i in CurrHandler.GetSpeakerIDs)
            {
                Debug.Log("speaker : " + i);
                characters[i].StartTalking();
            }
        }

        if (CurrHandler.Ended(timer))
        {
            foreach (var i in CurrHandler.GetSpeakerIDs)
                characters[i].StopTalking();
            NextHandler();
        }
    }

    public SpeechHandler CurrHandler
    {
        get
        {
            return handlers[handlerIndex];
        }
    }

    public void NextHandler()
    {
        ++handlerIndex;
    }

    public void EndSpeeches()
    {
        playing = false;

        audioSource.Stop();
        foreach (var character in characters)
            character.StopTalking();

        StartCoroutine(Upload());
    }

    public void Init()
    {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        yield return DownloadSpeeches();

        yield return GetAudioClip();

        float l = audioSource.clip.length;

        handlers = new SpeechHandler[speeches.Length];
        for (int i = 0; i < speeches.Length; i++)
        {
            handlers[i] = new SpeechHandler(speeches[i]);
        }

        displayTimeline.Init(startTime, endTime, duration);

        yield return new WaitForEndOfFrame();



        DisplayLoading.Instance.FadeOut();

        yield return new WaitForSeconds(1f);

        GameManager.Instance.ShowGameUI();

        yield return new WaitForSeconds(1f);

        audioSource.Play();
        timer = 0f;
        playing = true;

    }

    public void Pause()
    {
        playing = false;
        audioSource.Pause();

        DisplayPause.Instance.FadeInInstant();

        foreach (var item in characters)
        {
            item.GetAnimator.enabled = false;
            item._faceControl.Stop();
        }
    }

    public void Resume()
    {
        playing = true;
        audioSource.UnPause();
        DisplayPause.Instance.FadeOut();

        foreach (var i in CurrHandler.GetSpeakerIDs)
            characters[i].StartTalking();
    }

    public int GetSpeakerID(Speech _speech)
    {
        string str = _speech.speaker[0].Remove(0, _speech.speaker[0].Length - 1);
        return int.Parse(str);
    }

    IEnumerator Upload()
    {
        GameManager.Instance.HideGameUI();

        yield return new WaitForSeconds(1f);

        DisplayLoading.Instance.FadeIn();

        yield return new WaitForSeconds(1f);

        string url = $"https://oopaip.ina.fr/oopaip/api/python/set_annotations?{User.main.GetAuth}";

        string json = JsonConvert.SerializeObject(speeches);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", jsonBytes);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }

        yield return new WaitForSeconds(1f);

        DisplayLoading.Instance.FadeOut();

        DisplayStartLevel.Instance.FadeIn();
    }


    IEnumerator DownloadSpeeches()
    {
        string uri = $"https://oopaip.ina.fr/oopaip/api/python/next_job?login={User.main.user_login}";
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            string txt = www.downloadHandler.text;
            speeches = JsonConvert.DeserializeObject<Speech[]>(txt);
        }
    }

    IEnumerator GetAudioClip()
    {
        int startIndex = speeches[0].stream.IndexOf('#')+3;
        string timecode_start = speeches[0].stream.Remove(0, startIndex);
        int lastIndex = timecode_start.LastIndexOf(',');
        timecode_start = timecode_start.Remove(lastIndex);

        Debug.Log("start " + timecode_start);
        startTime = SpeechHandler.GetTimeCode(timecode_start);
        Debug.Log("float : " + startTime);
        string endStream = speeches[speeches.Length - 1].stream;
        string timecode_end = endStream.Remove(0,endStream.LastIndexOf(',')+1);
        Debug.Log("end " + timecode_end);
        endTime = SpeechHandler.GetTimeCode(timecode_end);
        Debug.Log("float : " + endTime);
        duration = endTime - startTime;

        string path = speeches[0].stream;

        Debug.LogError("audio path");
        Debug.Log(path);
        int start = path.IndexOf("//");
        string auth = $"{User.main.user_login}:{User.main.user_password}@";
        path = path.Insert(start + 2, auth);
        Debug.Log(path);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
            }

        }

    }

    public void StartEvent()
    {
        //CurrCharacter.StartTalking();
    }

    public void EndEvent()
    {
        //CurrCharacter.StopTalking();
    }
}
