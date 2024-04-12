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

    public int CompareTo(RankData<T> other)         //���� Ÿ�԰� ũ���
    {                                               //return���:: 1 ����ũ��, 0 ����, -1 �����۴�
        if (other == null)                          //����ִ°Ͱ� ��, ����� �۴�
            return 1;

        return Data.CompareTo(other.Data);          //Data���� �������� ������ ���Ѵ�.
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
