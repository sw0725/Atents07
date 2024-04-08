using System;
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

    public Action onFlagUse;
    public Action onFlagReturn;
    public bool IsFlag => CoverState == CellCoverState.Flag;

    List<Cell> neighbors;

    SpriteRenderer cover;
    SpriteRenderer inside;

    int aroundMineCount;
    bool isOpen = false;

    enum CellCoverState 
    {
        None,
        Flag,
        Question
    }
    CellCoverState coverState = CellCoverState.None;

    CellCoverState CoverState 
    {
        get => coverState;
        set 
        {
            coverState = value;
            switch (coverState)
            {
                case CellCoverState.None:
                    cover.sprite = Board[CloseCellType.Close];
                    break;
                case CellCoverState.Flag:
                    cover.sprite = Board[CloseCellType.Flag];
                    onFlagUse?.Invoke();
                    break;
                case CellCoverState.Question:
                    cover.sprite = Board[CloseCellType.Question];
                    onFlagReturn?.Invoke();
                    break;
            }
        }
    }

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        cover = c.GetComponent<SpriteRenderer>();
        c = transform.GetChild(1);
        inside = c.GetComponent<SpriteRenderer>();
    }

    public void InitialIze() 
    {
        neighbors = Board.GetNeightbors(ID);
    }

    public void ResetData()
    {
        hasMine = false;
        aroundMineCount = 0;
        isOpen = false;

        cover.sprite = Board[CloseCellType.Close];
        inside.sprite = Board[OpenCellType.Empty];
        cover.gameObject.SetActive(true);
    }

    public void SetMine()
    {
        hasMine = true;
        inside.sprite = Board[OpenCellType.Mine];

        foreach (Cell cell in neighbors)
        {
            cell.IncreaseAroundMineCount();
        }
    }

    void IncreaseAroundMineCount()
    {
        if (!hasMine)
        {
            aroundMineCount++;
            inside.sprite = Board[(OpenCellType)aroundMineCount];
        }
    }

    public void CellRightPress() 
    {
        switch (CoverState)
        {
            case CellCoverState.None:
                CoverState = CellCoverState.Flag;
                break;
            case CellCoverState.Flag:
                CoverState = CellCoverState.Question;
                break;
            case CellCoverState.Question:
                CoverState = CellCoverState.None;
                break;
        }
    }

    public void LeftPress() 
    {
        if (isOpen) 
        {
            foreach (var cell in neighbors)
            {
                cell.LeftPress();
            }
        }
        else 
        {
            switch (CoverState)
            {
                case CellCoverState.None:
                    cover.sprite = Board[CloseCellType.ClosePress];
                    break;
                case CellCoverState.Question:
                    cover.sprite = Board[CloseCellType.QuestionPress];
                    break;
                default:
                    break;
            }
        }
    }

    public void LeftRelease() 
    {
        Open();
    }

    void Open() 
    {
        if (!isOpen && !IsFlag) 
        {
            isOpen = true;
            cover.gameObject.SetActive(false);
            int flagcount = 0;
            if (aroundMineCount <= 0)
            {
                foreach (var cell in neighbors)
                {
                    cell.Open();
                }
            }
            else if (hasMine) 
            {
                Debug.Log("Claer");
            }
            foreach (var cell in neighbors)
            {
                if (cell.IsFlag) flagcount++;
            }
            if (flagcount == aroundMineCount)
            {
                foreach (var cell in neighbors)
                {
                    if (!cell.IsFlag)
                    {
                        cell.Open();
                    }
                }
            }
            else 
            {
                foreach (var cell in neighbors)
                {
                    cell.RestoreCover();
                }
            }
        }
    }

    public void RestoreCover() 
    {
        if (isOpen)
        {
            foreach (var cell in neighbors)
            {
                cell.RestoreCover();
            }
        }
        else 
        {
            switch (CoverState)
            {
                case CellCoverState.None:
                    cover.sprite = Board[CloseCellType.Close];
                    break;
                case CellCoverState.Question:
                    cover.sprite = Board[CloseCellType.Question];
                    break;
                default:
                    break;
            }
        }
    }

#if UNITY_EDITOR
    public void Test_OpenCover() 
    {
        cover.gameObject.SetActive(false);
    }
#endif
}
