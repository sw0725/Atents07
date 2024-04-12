using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameInputField : MonoBehaviour
{
    TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void SetPlayerName(string name) 
    {
        inputField.text = name;         //인풋필드에 써있는 텍스트를 바꿈
    }

    public string GetPlayerName() 
    {
        return inputField.text;         //인풋필드에 써있는 텍스트를 가져욤 
    }
}
