using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TabSubPanel : MonoBehaviour
{
    public enum RankType 
    {
        Action =0,
        Time
    }
    public RankType rankType = RankType.Action;

    RankLine[] rankLines;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
    }

    public void Start()
    {
        GameManager.Instance.onGameClear += Refresh;
        Refresh();
    }

    void Refresh() 
    {
        RankDataManager rankDataManager = GameManager.Instance.RankDataManager;
        int index = 0;
        switch (rankType) 
        {
            case RankType.Action:
                foreach (var data in rankDataManager.ActionRank)
                {
                    rankLines[index].SetData<int>(index + 1, data.Data, data.Name);
                    index++;
                }
                break;
            case RankType.Time:
                foreach (var data in rankDataManager.TimeRank)
                {
                    rankLines[index].SetData<float>(index + 1, data.Data, data.Name);
                    index++;
                }
                break;
        }

        for (int i = index; i < rankLines.Length; i++) 
        {
            rankLines[i].ClearLine();
        }
    }
}
