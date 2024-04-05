using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action<int> onTimeChange;        //초단위 알림

    public float ElapsedTime => elapsedTime;
    float elapsedTime = 0.0f;

    int displayTime = -1;
    int DisplayTime 
    {
        get => displayTime;
        set 
        {
            if(displayTime != value) 
            {
                displayTime = value;
                onTimeChange?.Invoke(displayTime);
            }
        }
    }

    IEnumerator timeCoroutine;

    private void Start()
    {
        GameManager manager = GameManager.Instance;
        manager.onGameReady += TimerReset;
        manager.onGamePlay += TimerReset;
        manager.onGamePlay += Play;
        manager.onGameClear += Stop;
        manager.onGameOver += Stop;

        timeCoroutine = TimeProcess();
        DisplayTime = 0;
    }

    void Play() 
    {
        StartCoroutine(timeCoroutine);
    }

    void Stop() 
    {
        StopCoroutine(timeCoroutine);
    }

    void TimerReset() 
    {
        elapsedTime = 0.0f;
        DisplayTime = 0;
        StopCoroutine(timeCoroutine);
    }

    IEnumerator TimeProcess() 
    {
        while (true) 
        {
            elapsedTime += Time.deltaTime;
            DisplayTime = (int)elapsedTime;
            yield return null;
        }
    }
}
