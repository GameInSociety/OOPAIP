using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Invoke("Init", 0f);
    }

    void Init()
    {
        string login = PlayerPrefs.GetString("login", "none");
        Debug.Log(login);
        if (login != "none")
        {
            string password = PlayerPrefs.GetString("password", "none");
            DisplayLogin.instance.Display(login, password);
        }
        else
        {
            DisplayUser.Instance.FadeIn();
        }
    }

    public void ShowGameUI()
    {
        DisplayCamera.Instance.FadeIn();
        LabelizeButton.Instance.FadeIn();

    }

    public void HideGameUI()
    {
        DisplayCamera.Instance.FadeOut();
        LabelizeButton.Instance.FadeOut();
    }

    public void StartLevel()
    {
        DisplayStartLevel.Instance.FadeOut();
        SpeechManager.instance.Init();
    }

}
