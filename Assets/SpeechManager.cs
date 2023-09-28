using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine;
using System.Text;

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
            timer = endTime -2f;

        if ( timer >= endTime)
            EndSpeeches();

        if ( handlerIndex >= handlers.Length)
            return;

        if (CurrHandler.Started(timer))
        {
            foreach (var i in characters)
                i.StopTalking();

            foreach (var i in CurrHandler.GetSpeakerIDs)
            {
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

        if ( handlerIndex >= handlers.Length)
        {
            EndSpeeches();
        }
    }

    public void EndSpeeches()
    {
        SetPlay(false);
        handlerIndex = 0;

        audioSource.Stop();
        foreach (var character in characters)
            character.StopTalking();

        Debug.Log("ENDING SPEECH");
        StartCoroutine(Upload());
    }

    public void Init()
    {
        Debug.Log("init speech manager");
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        SetPlay(false);
        DisplayLoading.Instance.FadeIn();

        yield return DownloadSpeeches();

        yield return GetAudioClip();

        float l = audioSource.clip.length;

        handlers = new SpeechHandler[speeches.Length];
        for (int i = 0; i < speeches.Length; i++)
        {
            handlers[i] = new SpeechHandler(speeches[i]);
        }

        displayTimeline.Init(startTime, endTime, duration);
        handlerIndex = 0;
        timer = startTime;

        yield return new WaitForEndOfFrame();

        DisplayLoading.Instance.FadeOut();

        yield return new WaitForSeconds(1f);

        GameManager.Instance.ShowGameUI();

        yield return new WaitForSeconds(1f);

        audioSource.Play();
        
        SetPlay(true);

    }

    public void SetPlay(bool play)
    {
        playing = play;
        Debug.Log("playing : " + playing);
    }

    public void Pause()
    {
        SetPlay(false);
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
        SetPlay(true);
        audioSource.UnPause();
        DisplayPause.Instance.FadeOut();

        foreach (var i in CurrHandler.GetSpeakerIDs)
            characters[i].StartTalking();
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
                DisplayMessage.Instance.Display(www.error);
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
            DisplayMessage.Instance.Display(www.error);
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

        startTime = SpeechHandler.GetTimeCode(timecode_start);
        string endStream = speeches[speeches.Length - 1].stream;
        string timecode_end = endStream.Remove(0,endStream.LastIndexOf(',')+1);
        endTime = SpeechHandler.GetTimeCode(timecode_end);
        duration = endTime - startTime;

        string path = speeches[0].stream;

        int start = path.IndexOf("//");
        string auth = $"{User.main.user_login}:{User.main.user_password}@";
        path = path.Insert(start + 2, auth);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                DisplayMessage.Instance.Display(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
                audioSource.time = startTime;
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
