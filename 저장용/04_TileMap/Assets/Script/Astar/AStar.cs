using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    const float sideDistance = 1.0f;
    const float diagonalDistance = 1.4f;

    public static List<Vector2Int> PathFine(GridMap map, Vector2Int start, Vector2Int end) 
    {
        List<Vector2Int> path = null;

        if (map.isValidPosition(start) && map.isValidPosition(end) && map.IsWall(start) && map.IsWall(end)) 
        {
            map.ClearMapData();

            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();

            Node current = map.GetNode(start);
            current.G = 0.0f;
            current.H = GetHeuristic(current, end);
            open.Add(current);

            while (open.Count > 0) 
            {
                
            }

            if (current == end) 
            {
                
            }
        }
        return path;
    }
                        //현위치에서 목적지까지 직선거리 계산
    private static float GetHeuristic(Node current, Vector2Int end) 
    {
        return Mathf.Abs(current.X - end.x) + Mathf.Abs(current.Y - end.y);
    }               //절대값반환
}
