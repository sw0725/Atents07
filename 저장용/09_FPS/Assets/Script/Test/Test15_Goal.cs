using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test15_Goal : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.HP -= 1000;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Goal goal = FindAnyObjectByType<Goal>();
        goal.SetRandomPos(GameManager.Instance.mazeWidth, GameManager.Instance.mazeHeight);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Goal goal = FindAnyObjectByType<Goal>();

        int size = GameManager.Instance.mazeWidth * GameManager.Instance.mazeHeight;
        int[] counter = new int[size];

        for(int i = 0; i < 10000; i++) 
        {
            Vector2Int result = goal.TestSetRandomPos(GameManager.Instance.mazeWidth, GameManager.Instance.mazeHeight);
            int index = result.x + result.y * GameManager.Instance.mazeWidth;
            counter[index]++;
        }

        for(int i = 0; i < size; i++) 
        {
            Debug.Log($"{i} : {counter[i]}");
        }
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Goal goal = FindAnyObjectByType<Goal>();

        goal.onGameClear += () => Debug.Log("Clear");
    }
}
