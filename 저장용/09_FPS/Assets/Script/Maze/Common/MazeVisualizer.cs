using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    public GameObject cellPrefab;

    public void Draw(Maze maze) 
    {
        float size = CellVisualizer.cellSize;
        foreach(var cell in maze.Cells) 
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * size, 0, -cell.Y * size);
            obj.gameObject.name = $"Cell_({cell.X}, {cell.Y})";

            CellVisualizer cellVisualizer = obj.GetComponent<CellVisualizer>();
            cellVisualizer.RefreshWall(cell.Path);
        }
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

    public Vector3 GridToWorld(int x, int y)
    {
        float size = CellVisualizer.cellSize;
        float sizeHalf = size * 0.5f;

        return new(size * x + sizeHalf, size * -y - sizeHalf);//
    }
}
