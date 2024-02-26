using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawPath(TileGridMap map, List<Vector2Int> path) 
    {
        if (map != null && path != null) 
        {
            lineRenderer.positionCount = path.Count;    //경로 수 만큼 라인랜더러의 위치 추가

            int index = 0;
            foreach (Vector2Int pos in path) 
            {
                Vector2 world = map.GridToWorld(pos);
                lineRenderer.SetPosition(index, world); //라인랜더러에 설정
                index++;
            }
        }
    }

    public void ClearPath()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
    }
}
