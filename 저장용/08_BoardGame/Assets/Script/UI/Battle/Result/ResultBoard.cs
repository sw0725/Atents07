using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultBoard : MonoBehaviour
{
    public Material victory;
    public Material defeat;

    TextMeshProUGUI result;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        result = c.GetComponent<TextMeshProUGUI>();
    }

    public void Toggle() 
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SetVictoryDefeat(bool isVictory) 
    {
        if (isVictory) 
        {
            result.text = "½Â¸®!";
            result.fontMaterial = victory;
        }
        else 
        {
            result.text = "ÆÐ¹è...";
            result.fontMaterial = defeat;
        }
    }
}
