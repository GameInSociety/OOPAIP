using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    public static Voice instance;

    public AudioClip[] clips;
    public AudioSource source;

    public float minPitch = 1f;
    public float maxPitch = 1f;

    bool playing = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }

        if(playing)
        {
            if (source.isPlaying)
            {

            }
            else
            {
                
            }
        }
    }

    public void Play()
    {
        playing = true;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
    }

    public void Stop()
    {
        playing = false;
    }
}
