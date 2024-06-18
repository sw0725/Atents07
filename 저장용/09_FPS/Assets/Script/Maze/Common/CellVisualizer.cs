using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellVisualizer : MonoBehaviour
{
    public const float CellSize = 10.0f;

    GameObject[] walls;
    
    GameObject[] coners;    //

    private void Awake()
    {
        Transform c = transform.GetChild(1);
        walls = new GameObject[c.childCount];
        for (int i = 0; i < walls.Length; i++) 
        {
            walls[i] = c.GetChild(i).gameObject;
        }

        c = transform.GetChild(2);                  //
        coners = new GameObject[c.childCount];
        for (int i = 0; i < walls.Length; i++)
        {
            coners[i] = c.GetChild(i).gameObject;
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

    public void RefreshConer(Cell[] neghbor)                 //�ϼ�, �ϵ�, ����, ����        �ϵ�����    //
    {
        int index = 0;
        foreach(Cell cell in neghbor) 
        {
            if (cell.IsPath((Direction)(3 + index)))
            {

            }
            else
            {

                coners[index].SetActive(false);
            }
            index++;
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
