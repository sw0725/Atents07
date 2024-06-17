using Cinemachine;
using System;
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

    public EnemySpawner EnemySpawner => enemySpawner;
    EnemySpawner enemySpawner;

    public int MazeWidth => mazeWidth;
    public int mazeWidth = 20;
    public int MazeHeight => mazeHeight;
    public int mazeHeight = 20;

    public Action onGameStart;
    public Action<bool> onGameClear;    //true면 클리어

    int killCount = 0;
    float playTime = 0.0f;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        Vector3 centerPos = MazeVisualizer.GridToWorld(mazeWidth / 2, mazeHeight / 2);
        player.transform.position = centerPos;
        player.onDie += GameOver;

        GameObject obj = GameObject.FindWithTag("FollowCamera");
        if (obj != null) 
        {
            followCamera = obj.GetComponent<CinemachineVirtualCamera>();
        }

        enemySpawner = FindAnyObjectByType<EnemySpawner>();

        generator = FindAnyObjectByType<MazeGenerator>();
        if(generator != null)
        {
            generator.Generate(MazeWidth, MazeHeight);
            generator.onMazeGenerated += () =>
            {
                enemySpawner?.EnemyAllSpawn();

                playTime = 0.0f;
                killCount = 0;
            };
        }

        ResultPanel resultPanel = FindAnyObjectByType<ResultPanel>();
        resultPanel.gameObject.SetActive(false);

        onGameClear += (isClear) =>
        {
            Crosshair crosshair = FindAnyObjectByType<Crosshair>();
            crosshair.gameObject.SetActive(false);
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

    public void GameStart() 
    {
        onGameStart?.Invoke();
    }

    public void GameClear() 
    {
        onGameClear?.Invoke(true);
    }

    public void GameOver() 
    {
        onGameClear.Invoke(false);
    }
}
