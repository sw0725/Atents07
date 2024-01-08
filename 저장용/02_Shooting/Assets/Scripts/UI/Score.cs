using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshPro score;
    private void Awake()
    {
        score = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        player.onScoreChange += RefershScore;
    }

    private void RefershScore(int newScore)
    {
        score.text = $"Score : {newScore:d5}";
    }
}
