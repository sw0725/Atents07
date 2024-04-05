using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int ID
    {
        get => id.GetValueOrDefault();      //디펄트는 0, 즉 id가 0이라면 널일 수도 잇다.
        set
        {
            if (id == null)                 //한번만 설정할것임
            {
                id = value;
            }
        }
    }
    int? id = null;             //이니셜라이즈에서 아이디가 설정되므로 ?로 설정

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
