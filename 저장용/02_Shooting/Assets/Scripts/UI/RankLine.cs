using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI nameText;
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        Transform c = transform.GetChild(1);
        nameText = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(2);
        scoreText = c.GetComponent<TextMeshProUGUI>();
    }

    public void SetData(string name, int score) 
    {
        nameText.text = name;
        scoreText.text = score.ToString("N0");      //숫자 세자리마다 콤마
    }
}
