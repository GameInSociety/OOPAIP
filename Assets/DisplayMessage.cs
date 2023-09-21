using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class DisplayMessage : Displayable
{
    public static DisplayMessage Instance;

    public TextMeshProUGUI uiText;

    private void Awake()
    {
        Instance = this;
    }

    public void Display(string message)
    {
        FadeInInstant();
        uiText.text = message;

    }
}
