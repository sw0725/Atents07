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
        if (nowScore < preScore)    // ������ �ö󰡴� ������ ��
        {
            float speed = Mathf.Max((preScore - nowScore) * 5.0f, upSpeed);   // �ּ� scoreUpSpeed ����

            nowScore += Time.deltaTime * speed;
            nowScore = Mathf.Min(nowScore, preScore);

            int temp = (int)nowScore;
            score.text = $"Score : {temp:d5}";
            //score.text = $"Score : {currentScore:f0}";    // �Ҽ��� ��� ���ϱ�
        }
    }

    private void RefreshScore(int newScore)
    {
        preScore = newScore;
    }
}
