using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Board : TestBase
{
    public Board Board;

    protected override void OnLClick(InputAction.CallbackContext context)
    {
        Vector2Int grid = Board.GetMouseGridPosition();
        Debug.Log($"{grid.x}, {grid.y}");
        if (Board.IsInBoard(grid)) 
        {
            Debug.Log("In");
        }
        else 
        {
            Debug.Log("Out");
        }
        Vector3 world = Board.GridToWorld(grid);
        Debug.Log($"{world}");
    }
}
