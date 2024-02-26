using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AStar : TestBase
{
    public int width = 7; 
    public int height = 7;

    public Vector2Int start;
    public Vector2Int end;

    GridMap map;

    private void Start()
    {
        map = new GridMap(width, height);

        Node node;
        node = map.GetNode(map.IndexToGrid(17));
        node.type = Node.NodeType.Wall;

        node = map.GetNode(map.IndexToGrid(24));
        node.type = Node.NodeType.Wall;

        node = map.GetNode(map.IndexToGrid(31));
        node.type = Node.NodeType.Wall;

        start = map.IndexToGrid(22);
        end = map.IndexToGrid(26);
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

    protected override void OnTest1(InputAction.CallbackContext _)
    {
        List<Vector2Int> path = AStar.PathFind(map, start, end);
        PrintList(path);
    }
}
