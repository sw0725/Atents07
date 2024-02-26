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
    {   //Ÿ�ϸ��� �׸�����ǥ ���ϱ�
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int startPosition = (Vector2Int)background.WorldToCell(worldPosition);

        if (!IsWall(startPosition)) 
        {
            start = startPosition;
        }
    }

    protected override void OnRClick(InputAction.CallbackContext context)
    {   //��ġ�� Ÿ���� �ִ��� ������ Ȯ�� 
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
    {   //����� ��ġ�� �ٽô� �پ���� �ʴ´�.
        //background.size.x;      //��׶��� ���ο� �鰡�ִ� �� ��(���α���) 
        Debug.Log($"background : {background.size.x}, {background.size.y}");
        Debug.Log($"background : {obstacle.size.x}, {obstacle.size.y}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {   //������ ���� �Ʒ��� �ִ�.
        Debug.Log($"origin : {background.origin}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {   // min : ���� ���ʾƷ� ��ǥ max: ���� ������ �� ��ǥ
        Debug.Log($"Background : {background.cellBounds.min}, {background.cellBounds.max}");
    }
}
