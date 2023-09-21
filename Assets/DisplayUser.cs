using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

public class DisplayUser : Displayable
{
    public static DisplayUser Instance;

    public CanvasGroup launchCanvasGroup;
    public TMP_InputField inputField_name;
    public TMP_InputField inputField_user_login;
    public TMP_InputField inputField_login_god;
    public TMP_InputField inputField_password_god;

    private void Awake()
    {
        Instance = this;
    }


    public override void Show()
    {
        base.Show();
        Debug.Log("duk");
    }

    public override void Start()
    {
        base.Start();

        OnEndEdit();
    }

    public void OnEndEdit()
    {
        launchCanvasGroup.alpha = 0.5f;
        launchCanvasGroup.interactable = false;

        if (string.IsNullOrEmpty(inputField_user_login.text))
            return;
        if (string.IsNullOrEmpty(inputField_login_god.text))
            return;
        if (string.IsNullOrEmpty(inputField_password_god.text))
            return;
        if (string.IsNullOrEmpty(inputField_name.text))
            return;

        launchCanvasGroup.alpha = 1f;
        launchCanvasGroup.interactable = true;

    }

    public void Register()
    {
        StartCoroutine(RegisterCoroutine());
    }

    IEnumerator RegisterCoroutine()
    {
        string god_login = inputField_login_god.text;
        string god_password = inputField_password_god.text;
        string user_login = inputField_user_login.text;
        string user_name = inputField_name.text;

        god_password = UnityWebRequest.EscapeURL(god_password);
        user_name = UnityWebRequest.EscapeURL(user_name);

        string url = $"https://oopaip.ina.fr/oopaip/api/python/register_user?login_god={god_login}&passwd_god={god_password}&user_login={user_login}&name={user_name}";
        Debug.Log(url);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            DisplayMessage.Instance.Display(www.error);
        }
        else
        {
            // Show results as text
            string login = inputField_user_login.text;
            string password = www.downloadHandler.text;
            Debug.Log("password : " + password);
            DisplayMessage.Instance.Display($"Votre mot de passe : \n{password}");
            DisplayLogin.instance.Display(login, password);

            PlayerPrefs.SetString("login", login);
            PlayerPrefs.SetString("password", password);
            //gsarnikovCans%du26sePt

        }
    }

    public void HandleOnLaunchButton()
    {
        Register();
    }
}
