using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public float TurnDuration => turnDuration;
    public float turnDuration = 5.0f;
    public bool timeOutEnable = true;

    public Action<int> onTurnStart;
    public Action onTurnEnd;

    bool isEndProcess = false;
    float turnRemainTime = 0.0f;
    int turnNumber = 1;
    bool isTurnEnable = true;

    enum TurnProcessState 
    {
        None,               //둘다 행동 중
        One,
        Both
    }
    TurnProcessState state = TurnProcessState.None;

    public void OnInitialize(PlayerBase user, PlayerBase enemy)
    {
        turnNumber = 0;

        if (!timeOutEnable) 
        {
            turnDuration = float.MaxValue;
        }
        turnRemainTime = TurnDuration;

        state = TurnProcessState.None;
        isTurnEnable = true;

        onTurnStart = null;
        onTurnEnd = null;

        if(user != null)
        {
            user.onActionEnd += PlayerTurnEnd;
            user.onDefeat += TurnManagerStop;
        }

        if(enemy != null)
        {
            enemy.onActionEnd += PlayerTurnEnd;
            enemy.onDefeat += TurnManagerStop;
        }

        OnTurnStart();
    }

    private void Update()
    {
        turnRemainTime -= Time.deltaTime;
        if(isTurnEnable && turnRemainTime < 0.0f) 
        {
            OnTurnEnd();
        }
    }

    void OnTurnStart() 
    {
        if (isTurnEnable) 
        {
            turnNumber ++;
            Debug.Log($"{turnNumber} start");
            state = TurnProcessState.None;
            turnRemainTime = TurnDuration;

            onTurnStart?.Invoke(turnNumber);
        }
    }

    void OnTurnEnd()
    {
        if (isTurnEnable)
        {
            isEndProcess = true;
            onTurnEnd?.Invoke();
            Debug.Log($"{turnNumber} end");

            isEndProcess = false;
            OnTurnStart();
        }
    }

    void PlayerTurnEnd() 
    {
        if(!isEndProcess) 
        {
            state++;
            if(state >= TurnProcessState.Both) 
            {
                OnTurnEnd();
            }
        }
    }

    void TurnManagerStop(PlayerBase _) 
    {
        isTurnEnable = false;
    }
}
