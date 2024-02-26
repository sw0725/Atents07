using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_AStar_Tilemap : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;

    public Vector2Int start;
    public Vector2Int end;

    public PathLine pathLine;

    TileGridMap map;

    private void Start()
    {
        map = new TileGridMap(background, obstacle);
        pathLine.ClearPath();
    }

    bool IsWall(Vector2Int gridPosition) 
    {
        TileBase tileBase = obstacle.GetTile((Vector3Int)gridPosition);
        return tileBase != null;
    }

    private void PrintList(List<Vector2Int> list)
    {
        string str = "";
        foreach (Vector2Int v in list)
        {
            str += $"{v} -> ";
        }
        Debug.Log(str + "END");
    }

    protected override void OnLClick(InputAction.CallbackContext context)
    {   //타일맵의 그리드좌표 구하기
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int startPosition = (Vector2Int)background.WorldToCell(worldPosition);

        if (!IsWall(startPosition)) 
        {
            start = startPosition;
        }
    }

    protected override void OnRClick(InputAction.CallbackContext context)
    {   //위치에 타일이 있는지 없는지 확인 
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int gridPosition = (Vector2Int)background.WorldToCell(worldPosition);

        if (!IsWall(gridPosition))
        {
            end = gridPosition;

            List<Vector2Int> path = AStar.PathFind(map, start, end);
            pathLine.DrawPath(map, path);
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {   //사이즈가 넘치면 다시는 줄어들지 않는다.
        //background.size.x;      //백그라운드 가로에 들가있는 셀 수(가로길이) 
        Debug.Log($"background : {background.size.x}, {background.size.y}");
        Debug.Log($"background : {obstacle.size.x}, {obstacle.size.y}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {   //원점은 왼쪽 아래에 있다.
        Debug.Log($"origin : {background.origin}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {   // min : 가장 왼쪽아래 좌표 max: 가장 오른쪽 위 좌표
        Debug.Log($"Background : {background.cellBounds.min}, {background.cellBounds.max}");
    }
}
