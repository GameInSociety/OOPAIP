using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayLogin : Displayable
{
    public static DisplayLogin instance;

    public TextMeshProUGUI uiText;

    public TMP_InputField inputField_user_login;
    public TMP_InputField inputField_user_password;

    public CanvasGroup launchCanvasGroup;

    private void Awake()
    {
        //https://oopaip.ina.fr/oopaip/api/python/get_discourse_speechturn?media=http://www.ina.fr/media/SA_706921_008&format=ogg&simplified_uri=false&groundtruth=false

        //https://oopaip.ina.fr/oopaip/api/python/get_discourse_speechturn?media=http://www.ina.fr/media/SA_706921_008&simplified_uri=false&groundtruth=false&user=dudule&start=90&duration=190

        instance = this;
    }

    public void Display(string _username, string _password)
    {
        inputField_user_login.text = _username;
        inputField_user_password.text = _password;

        FadeIn();
    }

    public void OnEndEdit()
    {
        launchCanvasGroup.alpha = 0.5f;
        launchCanvasGroup.interactable = false;

        if (string.IsNullOrEmpty(inputField_user_login.text))
        {
            return;
        }
        if (string.IsNullOrEmpty(inputField_user_password.text))
        {
            return;
        }

        launchCanvasGroup.alpha = 1f;
        launchCanvasGroup.interactable = true;
    }


    public void ResetSave(){
        PlayerPrefs.DeleteAll();
        FadeOut();
        DisplayUser.Instance.FadeIn();
    }

    public void HandleOnLaunchButton()
    {
        string username = inputField_user_login.text;
        string password = inputField_user_password.text;
        User.Init(username, password);

        PlayerPrefs.SetString("login", username);
        PlayerPrefs.SetString("password", password);

        FadeOut();
        DisplayStartLevel.Instance.FadeIn();
        
        
    }
}
