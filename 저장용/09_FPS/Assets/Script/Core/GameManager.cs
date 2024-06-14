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

    public Maze Maze => generator.Maze;
    MazeGenerator  generator;

    public int MazeWidth => mazeWidth;
    public int mazeWidth = 20;
    public int MazeHeight => mazeHeight;
    public int mazeHeight = 20;

    int killCount = 0;
    float playTime = 0.0f;

    protected override void OnInitialize()
    {
        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
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

                playTime = 0.0f;
                killCount = 0;
            };
        }

        ResultPanel resultPanel = FindAnyObjectByType<ResultPanel>();
        resultPanel.gameObject.SetActive(false);

        Goal goal = FindAnyObjectByType<Goal>();
        goal.onGameClear += () => 
        {
            crosshair.gameObject.SetActive(false);
            player.InputDisable();
            resultPanel.Open(true, killCount, playTime);     //Time.timeSinceLevelLoad : 씬이 로딩되고 지난 시간
        };

        Cursor.lockState = CursorLockMode.Locked;           //커서를 화면 중앙에 고정(안보입니당)
    }

    public void IncreaseKillCount() 
    {
        killCount++;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }
}
