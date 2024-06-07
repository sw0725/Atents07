using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualizer : MonoBehaviour
{
    public const float CellSize = 10.0f;

    GameObject[] walls;

    private void Awake()
    {
        Transform c = transform.GetChild(1);
        walls = new GameObject[c.childCount];
        for (int i = 0; i < walls.Length; i++) 
        {
            walls[i] = c.GetChild(i).gameObject;
        }
    }

    public void RefreshWall(byte data)          //북동남서 순서로 1이 세팅됫으면 길, 0이 세팅됫으면 벽
    {
        for (int i = 0; i < walls.Length; i++) 
        {
            int mask = 1 << i;
            walls[i].SetActive(!((data & mask) != 0));
        }
    }

    public Direction GetPath()                  //벽은0, 길은 1로세팅된 정보 반환
    {
        int mask = 0;       //<<연산은 int 반환임으로
        for (int i = 0; i < walls.Length; i++) 
        {
            if (!walls[i].activeSelf) 
            {
                mask |= 1 << i;
            }
        }
        return (Direction)mask;
    }
}
