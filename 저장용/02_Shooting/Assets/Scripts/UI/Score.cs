using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI score;
    int preScore =0;
    float nowScore;
    float upSpeed = 0.05f;

    private void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        player.onScoreChange += RefreshScore;
        
        preScore = 0;
        nowScore = 0;
        score.text = "Score : 00000";
    }

    private void Update()
    {
        if (nowScore < preScore)    // 점수가 올라가는 도중일 때
        {
            float speed = Mathf.Max((preScore - nowScore) * 5.0f, upSpeed);   // 최소 scoreUpSpeed 보장

            nowScore += Time.deltaTime * speed;
            nowScore = Mathf.Min(nowScore, preScore);

            int temp = (int)nowScore;
            score.text = $"Score : {temp:d5}";
            //score.text = $"Score : {currentScore:f0}";    // 소수점 출력 안하기
        }
    }

    private void RefreshScore(int newScore)
    {
        preScore = newScore;
    }
}
