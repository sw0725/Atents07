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

    public void RefreshWall(byte data)          //�ϵ����� ������ 1�� ���õ����� ��, 0�� ���õ����� ��
    {
        for (int i = 0; i < walls.Length; i++) 
        {
            int mask = 1 << i;
            walls[i].SetActive(!((data & mask) != 0));
        }
    }

    public Direction GetPath()                  //����0, ���� 1�μ��õ� ���� ��ȯ
    {
        int mask = 0;       //<<������ int ��ȯ������
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
