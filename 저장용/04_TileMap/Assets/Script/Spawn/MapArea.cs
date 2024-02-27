using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapArea : MonoBehaviour
{
    public TileGridMap GridMap => map;

    Tilemap background;
    Tilemap obstacle;
    TileGridMap map;

    Spawns[] Spawns;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        background = child.GetComponent<Tilemap>();

        child = transform.GetChild(1);
        obstacle = child.GetComponent<Tilemap>();

        map = new TileGridMap(background, obstacle);

        child = transform.GetChild(2);
        Spawns = child.GetComponentsInChildren<Spawns>();
    }

    public List<Node> CalcSpawnArea(Spawns spanwn) 
    {
        List<Node> result = new List<Node>();

        Vector2Int min = map.WorldToGrid(spanwn.transform.position);
        Vector2Int max = map.WorldToGrid(spanwn.transform.position + (Vector3)spanwn.size);

        for (int y = min.y; y < max.y; y++) 
        {
            for (int x = min.x; x < max.x; x++)
            {
                if (!map.IsWall(x, y)) 
                {
                    result.Add(map.GetNode(x, y));
                }
            }
        }
        return result;
    }

    public Vector2 GridToWorld(int x, int y) 
    {
        return map.GridToWorld(new(x, y));
    }
}
