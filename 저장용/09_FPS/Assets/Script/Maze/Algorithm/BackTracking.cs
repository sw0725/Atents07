using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;

    public BackTrackingCell(int x, int y) : base(x, y) 
    {
        visited = false;
    }
}

public class BackTracking : Maze
{
    protected override void OnSpecificAlgorithmExcute()         //재귀적 백트래킹 알고리즘(Recurcive BackTracking)
    {
        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++) 
            {
                cells[GridToIndex(x, y)] = new BackTrackingCell(x, y);      //셀 생성
            }
        }

        int index = Random.Range(0, cells.Length);
        BackTrackingCell start = (BackTrackingCell)cells[index];
        start.visited = true;

        MakeRecursive(start.X, start.Y);
    }

    void MakeRecursive(int x, int y)                            //재귀처리용 함수  //스택메모리에 함수가 쌓이는데 이 콜스택은 한계가 있어 과다크기는 터지기 마련이다.
    {
        BackTrackingCell current = (BackTrackingCell)cells[GridToIndex(x, y)];

        Vector2Int[] dirs = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };
        Util.Shuffle(dirs);

        foreach(Vector2Int dir in dirs) 
        {
            Vector2Int newPos = new(x + dir.x, y + dir.y);

            if (IsInGrid(newPos)) 
            {
                BackTrackingCell neighbor = (BackTrackingCell)cells[GridToIndex(newPos)];
                if(!neighbor.visited) 
                {
                    neighbor.visited = true;
                    ConnectPath(current, neighbor);

                    MakeRecursive(neighbor.X, neighbor.Y);
                }
            }
        }
    }
}
