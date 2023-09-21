using System.Globalization;
using UnityEngine;

public class SpeechHandler
{
    private Speech _speech;
    private float endTime;
    private float startTime;
    bool started = false;
    bool ended = false;

    public SpeechHandler(Speech speech)
    {
        _speech = speech;
    }

    public void AddAnnotation(string str)
    {
        _speech.annotations.Add(str);

        Debug.Log("adding annotation : ");
        Debug.Log(str);
    }

    public int[] GetSpeakerIDs
    {
        get
        {
            int[] ids = new int[_speech.speaker.Length];

            for (int i = 0; i < _speech.speaker.Length; i++)
            {
                string str = _speech.speaker[i].Remove(0, _speech.speaker[i].Length - 1);
                ids[i] = int.Parse(str);
            }

            return ids;
            
        }
    }

    public bool Started(float time)
    {
        if (!started && time >= StartTime)
        {
            started = true;
            return true;
        }
        return false;
    }

    public bool Ended(float time)
    {
        if ( !ended && time >= EndTime)
        {
            ended = true;
            return true;
        }
        return false;
    }

    public float EndTime
    {
        get
        {
            if (endTime == 0)
                endTime = GetTimeCode(_speech.end);
            return endTime;
        }
    }

    public static float GetTimeCode(string str)
    {
        string seconds_Str = str.Remove(0, str.LastIndexOf(':') + 1);
        float seconds = float.Parse(seconds_Str, CultureInfo.InvariantCulture);
        string minutes_Str = str.Remove(0, str.IndexOf(':') + 1);
        minutes_Str = minutes_Str.Remove(minutes_Str.IndexOf(':'));
        float minutes = float.Parse(minutes_Str, CultureInfo.InvariantCulture);
        float f = (minutes * 60f) + seconds;
        return f;
    }

    public float StartTime
    {
        get
        {
            if (startTime == 0)
                startTime = GetTimeCode(_speech.start);
            return startTime;
        }
    }
}
