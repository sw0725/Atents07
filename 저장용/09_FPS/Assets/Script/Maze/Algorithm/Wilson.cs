using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilsonCell : Cell
{
    public WilsonCell next;

    public bool isMazeMember;

    public WilsonCell(int x, int y) : base(x, y)
    {
        next = null;
        isMazeMember = false;
    }
}

public class Wilson : Maze
{
    readonly Vector2Int[] dirs = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };

    protected override void OnSpecificAlgorithmExcute()
    {
        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++)
            {
                cells[GridToIndex(x,y)] = new WilsonCell(x,y);
            }
        }

        int[] notInMazeArray = new int[cells.Length];
        for(int i = 0; i < notInMazeArray.Length; i++)
        {
            notInMazeArray[i] = i;
        }
        Util.Shuffle(notInMazeArray);
        List<int> notInMaze = new List<int>(notInMazeArray);

        int firstIndex = notInMaze[0];
        notInMaze.RemoveAt(0);
        WilsonCell first = (WilsonCell)cells[firstIndex];
        first.isMazeMember = true;

        while (notInMaze.Count > 0)
        {
            int index = notInMaze[0];
            notInMaze.RemoveAt(0);
            WilsonCell current = (WilsonCell)cells[index];

            do
            {
                WilsonCell neighbor = GetNeighbor(current);
                current.next = neighbor;
                current = neighbor;
            }
            while (!current.isMazeMember);

            WilsonCell path = (WilsonCell)cells[index];
            while (path != current)
            {
                path.isMazeMember = true;
                notInMaze.Remove(GridToIndex(path.X, path.Y));
                ConnectPath(path, path.next);
                path = path.next;
            }
        }
    }

    WilsonCell GetNeighbor(WilsonCell cell) 
    {
        Vector2Int neighborPos;

        do
        {
            Vector2Int dir = dirs[Random.Range(0, dirs.Length)];
            neighborPos = new Vector2Int(cell.X + dir.x, cell.Y + dir.y);
        }
        while (!IsInGrid(neighborPos));

        return (WilsonCell)cells[GridToIndex(neighborPos)];
    }
}
