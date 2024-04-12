using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RankData<T> where T : IComparable
{
    public T Data => data;
    readonly T data;

    public string Name => name;
    readonly string name;

    public RankData(T data, string name)
    {
        this.data = data;
        this.name = name;
    }

    public int CompareTo(RankData<T> other)         //같은 타입간 크기비교
    {                                               //return경우:: 1 내가크다, 0 같다, -1 내가작다
        if (other == null)                          //비어있는것과 비교, 빈것이 작다
            return 1;

        return Data.CompareTo(other.Data);          //Data값을 기준으로 순서를 정한다.
    }
}

public class RankDataManager : MonoBehaviour
{
    const int RankCount = 10;

    List<RankData<int>> actionRank;
    List<RankData<float>> timeRank;

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
#endif
}
