using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI score;
    int preScore =0;
    int nowScore;
    float upSpeed = 0.05f;

    private void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        player.onScoreChange += RefershScore;
        
        preScore = 0;
        nowScore = 0;
        score.text = "Score : 00000";
    }

    IEnumerator ScoreUp() 
    {
        while (preScore != nowScore) 
        {
            float speed = Math.Min(1/(preScore - nowScore), upSpeed);
            preScore++;
            score.text = $"Score : {preScore:d5}";
            preScore = Math.Min(preScore, nowScore);
            yield return new WaitForSeconds(speed);
        }
    }

    private void RefershScore(int newScore)
    {
        nowScore = newScore;
        StartCoroutine(ScoreUp());
    }
}
