using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public Transform currentWayPoint => wayPoints[index];

    Transform[] wayPoints;

    int index = 0;

    private void Awake()
    {
        wayPoints = new Transform[transform.childCount];
        for (int i = 0; i < wayPoints.Length; i++) 
        {
            wayPoints[i] = transform.GetChild(i);   
        }
    }

    public Transform GetNextPoint()
    {
        index++;
        index %= wayPoints.Length;              //a % b�϶� ������ ����� ���� 0 ~ b-1 �����̴�
        return wayPoints[index];
    }
}
