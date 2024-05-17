using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployData
{
    ShipDirection direction;
    public ShipDirection Direction => direction;

    Vector2Int position;
    public Vector2Int Position => position;

    public ShipDeployData(ShipDirection direction, Vector2Int position) 
    {
        this.direction = direction;
        this.position = position;
    }
}
