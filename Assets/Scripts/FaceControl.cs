using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FaceControl : MonoBehaviour
{
    public Sprite[] eye_Sprites;
    public Sprite[] mouth_Sprites;
    public Sprite idleMouth_Sprite;
    public Sprite idleEye_Sprite;

    public SpriteRenderer[] eye_SpriteRenderers;
    public SpriteRenderer mouth_SpriteRenderer;

    public float eye_Rate = 1.0f;
    public float mouth_Rate = 1.0f;

    public bool playVoice = false;

    public void StartAnim()
    {
        Stop();
        InvokeRepeating("ChangeEyes", 0f, eye_Rate);
        InvokeRepeating("ChangeMouth", 0f, mouth_Rate);
    }

    public void ChangeEyes()
    {
        int spriteID = Random.Range(0, eye_Sprites.Length);
        eye_SpriteRenderers[0].sprite = eye_Sprites[spriteID];
        eye_SpriteRenderers[1].sprite = eye_Sprites[spriteID];
    }

    public void ChangeMouth()
    {
        if (playVoice)
            Voice.instance.Play();
        int spriteID = Random.Range(0, mouth_Sprites.Length);
        mouth_SpriteRenderer.sprite = mouth_Sprites[spriteID];
    }

    public void Stop()
    {
        eye_SpriteRenderers[0].sprite = idleEye_Sprite;
        eye_SpriteRenderers[1].sprite = idleEye_Sprite;
        mouth_SpriteRenderer.sprite = idleMouth_Sprite;

        if (playVoice)
            Voice.instance.Stop();

        CancelInvoke("ChangeEyes");
        CancelInvoke("ChangeMouth");
    }


}
