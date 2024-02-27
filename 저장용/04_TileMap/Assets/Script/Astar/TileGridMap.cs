using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridMap : GridMap
{
    Vector2Int[] moveablePosition;
    Vector2Int origin;

    Tilemap background;

    public TileGridMap(Tilemap background, Tilemap obstacle) 
    {
        this.background = background;
        this.width = background.size.x;
        this.height = background.size.y;

        origin = (Vector2Int)background.origin;

        nodes = new Node[width * height];

        Vector2Int min = (Vector2Int)background.cellBounds.min;
        Vector2Int max = (Vector2Int)background.cellBounds.max;

        List<Vector2Int> moveable = new List<Vector2Int>(width * height);       //배열 크기 따기위한 임시리스트
        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                if (GridToIndex(x, y, out int? index)) 
                {
                    Node.NodeType nodeType = Node.NodeType.Plain;
                    TileBase tile = obstacle.GetTile(new(x, y));    //장애물 맵에 타일존재시
                    if (tile != null)
                    {
                        nodeType = Node.NodeType.Wall;              //벽으로 설정
                    }
                    else 
                    {
                        moveable.Add(new(x, y));
                    }
                    nodes[index.Value] = new Node(x, y, nodeType);
                }
            }
        }
        moveablePosition = moveable.ToArray();
    }

    protected override int CalcIndex(int x, int y)
    {       //원점이 0,0이 아님   //위아래 뒤집혀있음-y좌표 뒤집기
        return (x - origin.x) + ((height - 1) - (y - origin.y)) * width;
    }

    public override bool isValidPosition(int x, int y)
    {
        return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition) 
    {
        return (Vector2Int)background.WorldToCell(worldPosition);
    }

    public Vector2 GridToWorld(Vector2Int gridPosition)
    {                                                                   //좌표를 타일의 가운데로 지정
        return background.CellToWorld((Vector3Int)gridPosition) + new Vector3(0.5f, 0.5f);
    }

    public Vector2Int GetRandomMoveableposition() 
    {
        int index = Random.Range(0, moveablePosition.Length);
        return moveablePosition[index];
    }
}
