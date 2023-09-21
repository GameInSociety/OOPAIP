using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class User
{
    public static User main;

    public string user_login;
    public string user_password;

    public static void Init(string userLogin, string userPassword)
    {
        main = new User();
        main.user_login = userLogin;
        main.user_password = userPassword;
    }

    public string GetAuth
    {
        get
        {
            return $"login={main.user_login}&passwd={main.user_password}";
        }
    }
    
    

}
