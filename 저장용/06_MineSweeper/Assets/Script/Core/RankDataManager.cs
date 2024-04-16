using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RankData<T> : IComparable<RankData<T>> where T : IComparable<T>
{                           //컴페러블한 랭킹데이터는 반드시 컴페러블한 t를 사용한다
    public T Data => data;
    readonly T data;

    public string Name => name;
    readonly string name;

    public RankData(T data, string name)
    {
        this.data = data;
        this.name = name;
    }

    public int CompareTo(RankData<T> other)
    {
        return Data.CompareTo(other.Data);          //수가 작은것이 앞, 큰것이 뒤
    }
}

[Serializable]              //메모리 할당시 배열의 메모리 주소를 일렬로 고정하여, ToJson가능하게 한다
public class RankSaveData 
{
    public int[] actionCountRank;
    public string[] actionCountRanker;

    public float[] playTimeRank;
    public string[] playTimeRanker;
}

public class RankDataManager : MonoBehaviour
{
    const int RankCount = 10;

    public List<RankData<int>> ActionRank => actionRank;
    List<RankData<int>> actionRank;
    
    public List<RankData<float>> TimeRank => timeRank;
    List<RankData<float>> timeRank;

    const string RankDataFolder = "Save";
    const string RankDataFileName = "Ranking.json";

    private void Awake()
    {
        actionRank = new List<RankData<int>>(RankCount + 1);        //랭킹이 가득찼을때 자신의 기록과 랭킹기록을 모두 넣어 정렬하기 위함, 마지막 기록은 비워진다.
        timeRank = new List<RankData<float>>(RankCount + 1);
    }

    private void Start()
    {
        LoadRankData();
        GameManager.Instance.onGameClear += () => UpdateData(GameManager.Instance.ActionCount, GameManager.Instance.PlayTime, GameManager.Instance.PlayerName);
    }

    void SaveRankData() 
    {
        RankSaveData data = new RankSaveData();
        data.actionCountRank = new int[actionRank.Count];
        data.actionCountRanker = new string[actionRank.Count];
        data.playTimeRank = new float[timeRank.Count];
        data.playTimeRanker = new string[timeRank.Count];

        int index = 0;
        foreach (var rankData in actionRank) 
        {
            data.actionCountRank[index] = rankData.Data;
            data.actionCountRanker[index] = rankData.Name;
            index++;
        }
        index = 0;
        foreach (var rankData in timeRank)
        {
            data.playTimeRank[index] = rankData.Data;
            data.playTimeRanker[index] = rankData.Name;
            index++;
        }

        string json = JsonUtility.ToJson(data);
        string path = $"{Application.dataPath}/{RankDataFolder}";        //Application.dataPath = 에디터 상에서의Asste폴더, 폴더상의 Data폴더
        if (!Directory.Exists(path))                                     //Directory : 폴더 편집 관련
        {
            Directory.CreateDirectory(path);                             //없으면 만들기
        }
        string fullPath = $"{path}/{RankDataFileName}";
        File.WriteAllText(fullPath, json);
    }

    void LoadRankData() 
    {
        string path = $"{Application.dataPath}/{RankDataFolder}";
        string fullPath = $"{path}/{RankDataFileName}";
        if (Directory.Exists(path) && File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            RankSaveData data = JsonUtility.FromJson<RankSaveData>(json);
            actionRank.Clear();
            timeRank.Clear();

            int count = data.actionCountRank.Length;
            for(int i = 0; i < count; i++) 
            {
                actionRank.Add(new(data.actionCountRank[i], data.actionCountRanker[i]));
            }
            count = data.playTimeRank.Length;
            for (int i = 0; i < count; i++)
            {
                timeRank.Add(new(data.playTimeRank[i], data.playTimeRanker[i]));
            }
        }
    }

    void UpdateData(int actionCount, float playTime, string rankerName) 
    {
        actionRank.Add(new(actionCount, rankerName));
        timeRank.Add(new(playTime, rankerName));

        actionRank.Sort();
        timeRank.Sort();

        if (actionRank.Count > RankCount) 
        {
            actionRank.RemoveAt(RankCount); //11번째 재거
        }
        if (timeRank.Count > RankCount)
        {
            timeRank.RemoveAt(RankCount); //11번째 재거
        }
        SaveRankData();
    }

#if UNITY_EDITOR
    public void Test_RankSetting()
    {
        actionRank = new List<RankData<int>>(10);
        actionRank.Add(new(1, "AAA"));
        actionRank.Add(new(10, "BBB"));
        actionRank.Add(new(20, "CCC"));
        actionRank.Add(new(30, "DDD"));
        actionRank.Add(new(40, "EEE"));

        timeRank = new List<RankData<float>>(10);
        timeRank.Add(new(1.0f, "AAA"));
        timeRank.Add(new(10, "BBB"));
        timeRank.Add(new(20, "CCC"));
        timeRank.Add(new(30, "DDD"));
        timeRank.Add(new(40, "EEE"));
    }

    public void Test_ActionUpdate(int rank, string name) 
    {
        actionRank.Add(new(rank, name));
        actionRank.Sort();
    }
    public void Test_TimeUpdate(float rank, string name)
    {
        timeRank.Add(new(rank, name));
        timeRank.Sort();
    }

    public void Test_Save() 
    {
        SaveRankData();
    }

    public void Test_Load() 
    {
        LoadRankData();
    }
#endif
}
