using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    public Player Player => player;
    Player player;

    public CinemachineVirtualCamera FollowCamera => followCamera;
    CinemachineVirtualCamera followCamera;

    public int MazeWidth => mazeWidth;
    public int mazeWidth = 20;
    public int MazeHeight => mazeHeight;
    public int mazeHeight = 20;

    public Maze Maze => generator.Maze;
    MazeGenerator  generator;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();

        GameObject obj = GameObject.FindWithTag("FollowCamera");
        if (obj != null) 
        {
            followCamera = obj.GetComponent<CinemachineVirtualCamera>();
        }

        generator = FindAnyObjectByType<MazeGenerator>();
        if(generator != null)
        {
            generator.Generate(MazeWidth, MazeHeight);
            generator.onMazeGenerated += () =>
            {
                Vector3 centerPos = MazeVisualizer.GridToWorld(mazeWidth / 2, mazeHeight / 2);
                player.transform.position = centerPos;
            };
        }
    }
}
