using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class KillCount : MonoBehaviour
{
    ImageNumber imageNumber;

    float preScore = 0;
    float nowScore =0;

    public float upSpeed = 10.0f;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onKillCountChange += KillCountChange;
    }
    private void Update()
    {
        if (nowScore < preScore)
        {
            nowScore += Time.deltaTime * upSpeed;
            nowScore = Mathf.Min(nowScore, preScore);
            imageNumber.Number = Mathf.FloorToInt(nowScore);
        }
    }

    private void KillCountChange(int obj)
    {
        preScore = obj;
    }
}
