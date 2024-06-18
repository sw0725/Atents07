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

    public void MakeMaze(int width, int height, int seed = -1) //�̷λ���
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

    protected virtual void OnSpecificAlgorithmExcute()      //�� �˰��� �������̵�
    {
        //cells����, cells���� ������ ����� �°� ����
    }

    protected void ConnectPath(Cell from, Cell to) 
    {
        Vector2Int dir = new(to.X - from.X, to.Y - from.Y);
        if (dir.x > 0)//��
        {
            from.MakePath(Direction.East);
            to.MakePath(Direction.West);
        }
        else if (dir.x < 0)//��
        {
            from.MakePath(Direction.West);
            to.MakePath(Direction.East);
        }
        else if (dir.y > 0)//��
        {
            from.MakePath(Direction.South);
            to.MakePath(Direction.North);
        }
        else if (dir.y < 0)//��
        {
            from.MakePath(Direction.North);
            to.MakePath(Direction.South);
        }
    }

    protected bool IsInGrid(Vector2Int grid)
    {
        return grid.x >= 0 && grid.y >= 0 && grid.x < Width && grid.y < Height;
    }

    protected bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    protected Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index/width);
    }
    public int GridToIndex(int x, int y)        //
    {
        return x + y * width;
    }
    protected int GridToIndex(Vector2Int grid)
    {
        return grid.x + grid.y * width;
    }
}
