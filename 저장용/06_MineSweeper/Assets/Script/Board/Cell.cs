using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int ID
    {
        get => id.GetValueOrDefault();      //����Ʈ�� 0, �� id�� 0�̶�� ���� ���� �մ�.
        set
        {
            if (id == null)                 //�ѹ��� �����Ұ���
            {
                id = value;
            }
        }
    }
    int? id = null;             //�̴ϼȶ������ ���̵� �����ǹǷ� ?�� ����

    public bool HasMine => hasMine;
    bool hasMine = false;

    public Board Board 
    {
        get => parentBoard;
        set 
        {
            if (parentBoard == null) 
            {
                parentBoard = value;
            }
        }
    }
    Board parentBoard = null;

    SpriteRenderer cover;
    SpriteRenderer inside;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        cover = c.GetComponent<SpriteRenderer>();
        c=transform.GetChild(1);
        inside = c.GetComponent<SpriteRenderer>();
    }

    public void ResetData() 
    {
        hasMine = false;
        cover.sprite = Board[CloseCellType.Close];
        inside.sprite = Board[OpenCellType.Empty];
        cover.gameObject.SetActive(true);
    }

    public void SetMine() 
    {
        hasMine = true;
        inside.sprite = Board[OpenCellType.Mine];
    }
#if UNITY_EDITOR
    public void Test_OpenCover() 
    {
        cover.gameObject.SetActive(false);
    }
#endif
}
