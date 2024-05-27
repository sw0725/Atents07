using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Flags]
public enum Direction : byte 
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,
}

public class Cell
{
    public byte Path => path;
    byte path;

    public int X => x;          //��->��
    protected int x;

    public int Y => y;          //��->��
    protected int y;

    public Cell(int x, int y) 
    {
        path = (byte)Direction.None;
        this.x = x;
        this.y = y;
    }

    public void MakePath(Direction direction) //�� �߰�
    {
        path |= (byte)direction;
    }

    public bool IsPath(Direction direction) //�ش� ������ ���ΰ�
    {
        return (path & (byte)direction) != 0;
    }

    public bool IsWall(Direction direction)
    {
        return (path & (byte)direction) == 0;
    }
}
