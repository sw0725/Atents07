using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public Cell[] Cells => cells;
    protected Cell[] cells;

    public int Width => width;
    protected int width; 

    public int Height => height;
    protected int height;

    public void MakeMaze(int width, int height, int seed = -1) //미로생성
    {
        this.width = width;
        this.height = height;

        if (seed != -1)
        {
            Random.InitState(seed);
        }

        cells = new Cell[Width * Height];

        OnSpecificAlgorithmExcute();
    }

    protected virtual void OnSpecificAlgorithmExcute()      //각 알고리즘별 오버라이드
    {
        //cells생성, cells안을 생성된 결과에 맞게 세팅
    }

    protected void ConnectPath(Cell from, Cell to) 
    {
        //A,B셀 사이벽 제거
    }

    protected bool IsInGrid(Vector2Int grid) 
    {
        return true;
    }

    protected bool IsInGrid(int x, int y)
    {
        return true;
    }

    protected Vector2Int IndexToGrid(int index)
    {
        return Vector2Int.zero;
    }
    protected int GridToIndex(int x, int y)
    {
        return -1;
    }
    protected int GridToIndex(Vector2Int grid)
    {
        return GridToIndex(grid.x, grid.y);
    }
}
