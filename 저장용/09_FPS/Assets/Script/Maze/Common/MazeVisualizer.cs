using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject goalPrefab;

    Maze maze = null;

    Dictionary<Direction, Vector2Int> neighborDir;
    (Direction, Direction)[] coners = null;

    private void Awake()
    {
        neighborDir = new Dictionary<Direction, Vector2Int>(4);
        neighborDir[Direction.North] = new Vector2Int(0, -1);
        neighborDir[Direction.East] = new Vector2Int(1, 0);
        neighborDir[Direction.South] = new Vector2Int(0, 1);
        neighborDir[Direction.West] = new Vector2Int(-1, 0);

        coners = new (Direction, Direction)[]
        {
            (Direction.North, Direction.West),
            (Direction.North, Direction.East),
            (Direction.South, Direction.East),
            (Direction.South, Direction.West),
        };
    }

    public void Draw(Maze maze) 
    {
        this.maze = maze;
        float size = CellVisualizer.CellSize;

        foreach(var cell in maze.Cells) 
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * size, 0, -cell.Y * size);
            obj.gameObject.name = $"Cell_({cell.X}, {cell.Y})";

            CellVisualizer cellVisualizer = obj.GetComponent<CellVisualizer>();
            cellVisualizer.RefreshWall(cell.Path);

            int conerMask = 0;
            for (int i = 0; i < coners.Length; i++) 
            {
                if (IsConerVisible(cell, coners[i].Item1, coners[i].Item2)) 
                {
                    conerMask |= 1 << i;
                }
            }
            cellVisualizer.RefreshConer(conerMask);
        }
        GameObject goalObj = Instantiate(goalPrefab, transform);
        Goal goal = goalObj.GetComponent<Goal>();
        goal.SetRandomPos(maze.Width, maze.Height);
    }

    public void Clear() 
    {
        while (transform.childCount > 0) 
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }

    public static Vector3 GridToWorld(int x, int y)
    {
        float size = CellVisualizer.CellSize;
        float sizeHalf = size * 0.5f;

        return new(size * x + sizeHalf, 0, size * -y - sizeHalf);//
    }

    public static Vector2Int WorldToGrid(Vector3 world) 
    {
        float size = CellVisualizer.CellSize;
        Vector2Int result = new((int)(world.x / size), (int)(-world.z / size));
        return result;
    }

    bool IsConerVisible(Cell cell, Direction dir1, Direction dir2) 
    {
        bool result = false;
        if(cell.ConerCheck(dir1, dir2)) 
        {
            Cell neighborCell1 = maze.GetCell(cell.X + neighborDir[dir1].x, cell.Y + neighborDir[dir1].y);
            Cell neighborCell2 = maze.GetCell(cell.X + neighborDir[dir2].x, cell.Y + neighborDir[dir2].y);

            result = neighborCell1.IsWall(dir2) && neighborCell2.IsWall(dir1);
        }
        return result;
    }
}
