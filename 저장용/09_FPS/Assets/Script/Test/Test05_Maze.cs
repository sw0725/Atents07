using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test05_Maze : TestBase
{
    [Header("¼¿")]
    public Direction direction;
    public CellVisualizer cellVisualizer;

    [Header("¹Ì·Î")]
    public MazeVisualizer backTracking;
    public int width = 5;
    public int height = 5;

    public MazeVisualizer eller;
    public MazeVisualizer wilson;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        cellVisualizer.RefreshWall((byte)direction);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Debug.Log(cellVisualizer.GetPath());
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        backTracking.Clear();

        BackTracking maze = new BackTracking();
        maze.MakeMaze(width, height, seed);
        backTracking.Draw(maze);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        eller.Clear();

        Eller maze = new Eller();
        maze.MakeMaze(width, height, seed);
        eller.Draw(maze);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        wilson.Clear();

        Wilson maze = new Wilson();
        maze.MakeMaze(width, height, seed);
        wilson.Draw(maze);
    }
}
