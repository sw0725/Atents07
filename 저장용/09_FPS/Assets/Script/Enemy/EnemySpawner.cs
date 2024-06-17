using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemyCount = 50;
    public GameObject enemyPrefab;

    int mazeWidth;
    int mazeHeight;

    Player player;
    Enemy[] enemies;

    private void Awake()
    {
        enemies = new Enemy[enemyCount];
    }

    void Start() 
    {
        mazeWidth = GameManager.Instance.MazeWidth;
        mazeHeight = GameManager.Instance.MazeHeight;
        player = GameManager.Instance.Player;

        EnemyAllSpawn();

        GameManager.Instance.onGameStart += EnemyAllPlay;
        GameManager.Instance.onGameClear += (_) => EnemyAllStop();
    }

    Vector3 GetRandomSpawnPosition(bool init = false) //플레이어 주변의 랜덤 위치 반환 // init = 플레이어가 존재하지 않음
    {
        Vector2Int playerPos;
        if (init)
        {
            playerPos = new(mazeWidth / 2, mazeHeight / 2);
        }
        else 
        {
            playerPos = MazeVisualizer.WorldToGrid(player.transform.position);
        }

        int x;
        int y;
        int limit = 100;
        do
        {
            int index = Random.Range(0, mazeHeight * mazeWidth);
            x = index / mazeWidth;
            y = index % mazeHeight;
            limit--;
            if (limit < 1) 
            {
                break;
            }
        } while (!(x < playerPos.x + 5 && x > playerPos.x - 5 && y < playerPos.y + 5 && y > playerPos.y - 5));

        Vector3 world = MazeVisualizer.GridToWorld(x, y);

        return world;
    }

    IEnumerator Respawn(Enemy target) 
    {
        yield return new WaitForSeconds(3);
        target.Respawn(GetRandomSpawnPosition());
    }

    public void EnemyAllSpawn() 
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, transform);
            obj.name = $"Enemy_{i}";
            Enemy enemy = obj.GetComponent<Enemy>();
            enemies[i] = enemy;
            enemy.onDie += (target) =>
            {
                GameManager.Instance.IncreaseKillCount();
                StartCoroutine(Respawn(target));
            };
            enemy.Respawn(GetRandomSpawnPosition(true), true);
        }
    }

    void EnemyAllStop() 
    {
        foreach (var enemy in enemies) 
        {
           enemy.Stop();   
        }
    }

    void EnemyAllPlay() 
    {
        foreach (var enemy in enemies)
        {
            enemy.Play();
        }
    }
}
