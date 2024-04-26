using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    public const int BoardSize = 10;

    public Vector2Int IndexToGrid(uint index) 
    {
        return new Vector2Int((int)index % BoardSize, (int)index / BoardSize);
    }

    public Vector3 IndexToWorld(uint index) 
    {
        return GridToWorld(IndexToGrid(index));
    }
    
    public int? GridToIndex(int x, int y) 
    {
        int? result = null;
        if (IsInBoard(x, y)) 
        {
            result = x + y * BoardSize;
        }

        return result;
    }

    public int? GridToIndex(Vector2Int grid) 
    {
        return GridToIndex(grid.x, grid.y);
    }

    public Vector3 GridToWorld(int x, int y) 
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return GridToWorld(grid.x, grid.y);
    }

    public Vector2Int WorldToGrid(Vector3 world) 
    {
        world.y = transform.position.y;             //x,z 차이만 구하고자
        Vector3 diff = world - transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }

    public bool IsInBoard(Vector3 world) 
    {
        world.y = transform.position.y;
        Vector3 diff = world - transform.position;

        return diff.x >= 0 && diff.x <= BoardSize && diff.z <= 0 && diff.z >= -BoardSize;
    }

    public bool IsInBoard(int x, int y) 
    {
        return x > -1 && x < BoardSize && y > -1 && y < BoardSize;
    }

    public bool IsInBoard(Vector2Int grid)
    {
        return IsInBoard(grid.x, grid.y);
    }

    public Vector2Int GetMouseGridPosition() 
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        return WorldToGrid(world);
    }
}
