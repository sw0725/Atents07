using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject goalPrefab;

    public void Draw(Maze maze) 
    {
        float size = CellVisualizer.CellSize;
        foreach(var cell in maze.Cells) 
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * size, 0, -cell.Y * size);
            obj.gameObject.name = $"Cell_({cell.X}, {cell.Y})";

            CellVisualizer cellVisualizer = obj.GetComponent<CellVisualizer>();
            cellVisualizer.RefreshWall(cell.Path);
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
}
