using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    public const int BoardSize = 10;

    ShipType[] shipInfo;    //���忡 ��ġ�� �� ���� ��ĭ�� none

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize];     //�ڵ� none ����
    }

    //�Լ� ��ġ===========================================================

    public bool ShipDeployment(Ship ship, Vector2Int grid) 
    {
        bool result = IsShipDeploymentAvailable(ship, grid, out Vector2Int[] gridPos);
        if (result) 
        {
            foreach(var pos in gridPos) 
            {
                shipInfo[GridToIndex(pos).Value] = ship.ShipType;
            }
            Vector3 world = GridToWorld(grid);
            ship.transform.position = world;
            ship.Deploy(gridPos);
        }
        return result;
    }

    public bool ShipDeployment(Ship ship, Vector3 wolrd)
    {
        return ShipDeployment(ship, WorldToGrid(wolrd));
    }

    public void UndoShipDeployment(Ship ship)
    {
        foreach(var pos in ship.Positions) 
        {
            shipInfo[GridToIndex(pos).Value] = ShipType.None;
        }
        ship.UnDeploy();
        ship.gameObject.SetActive(false);
    }

    public bool IsShipDeploymentAvailable(Ship ship, Vector2Int grid, out Vector2Int[] resultPos) 
    {
        bool result = true;
        Vector2Int dir = Vector2Int.zero;
        switch (ship.Direction) 
        {
            case ShipDirection.North:
                dir = Vector2Int.up;
                break;
            case ShipDirection.East:
                dir = Vector2Int.left;
                break;
            case ShipDirection.South:
                dir = Vector2Int.down;
                break;
            case ShipDirection.West:
                dir = Vector2Int.right;
                break;
        }
        resultPos = new Vector2Int[ship.Size];
        for (int i = 0; i < ship.Size; i++) 
        {
            resultPos[i] = grid + dir * i;
        }
        foreach(var pos in resultPos)               //����ġ ������ �������ִ� ��(resultPos)�� �����Ǹ� �ȵȴ�.
        {
            if (!IsInBoard(pos) || IsShipDeployedPosition(pos)) 
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public bool IsShipDeploymentAvailable(Ship ship, Vector2Int grid) 
    {
        return IsShipDeploymentAvailable(ship, grid, out _);        //out�� �ʿ��ѵ� �Ź����°� �ƴ϶� out�� ���� �����⸦ �ϳ� ������� ���⿡�� out����
    }

    public bool IsShipDeploymentAvailable(Ship ship, Vector3 wolrd)
    {
        return IsShipDeploymentAvailable(ship, WorldToGrid(wolrd), out _);
    }

    public ShipType GetShipTypeOnBoard(Vector2Int grid) 
    {
        ShipType result = ShipType.None;
        int? index = GridToIndex(grid);
        if (index != null)
        {
            result = shipInfo[GridToIndex(grid).Value];
        }
        return result;
    }

    public ShipType GetShipTypeOnBoard(Vector3 world)
    {
        return GetShipTypeOnBoard(WorldToGrid(world));
    }

    //��ǥ ��ȯ===========================================================

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
        world.y = transform.position.y;             //x,z ���̸� ���ϰ���
        Vector3 diff = world - transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }

    public bool IsInBoard(Vector3 world) 
    {
        world.y = transform.position.y;
        Vector3 diff = world - transform.position;

        return diff.x >= 0 && diff.x <= BoardSize && diff.z <= 0 && diff.z >= -BoardSize;
    }

    public Vector2Int GetMouseGridPosition() 
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        return WorldToGrid(world);
    }

    public bool IsInBoard(int x, int y) 
    {
        return x > -1 && x < BoardSize && y > -1 && y < BoardSize;
    }

    public bool IsInBoard(Vector2Int grid)
    {
        return IsInBoard(grid.x, grid.y);
    }

    bool IsShipDeployedPosition(Vector2Int grid) 
    {
        int? index = GridToIndex(grid);
        bool result;
        if (index.HasValue)
            result = shipInfo[index.Value] != ShipType.None;
        else 
            result = false;
        return result;
    }
}
