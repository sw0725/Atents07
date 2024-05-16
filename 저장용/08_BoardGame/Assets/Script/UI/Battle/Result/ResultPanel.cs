using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    ResultBoard board;
    ResultAnalysis userAnalysis;
    ResultAnalysis enemyAnalysis;

    UserPlayer user;
    EnemyPlayer enemy;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        Button boardOpen = c.GetComponent<Button>();

        c = transform.GetChild(1);
        board = c.GetComponent<ResultBoard>();

        c = board.transform.GetChild(1);
        userAnalysis = c.GetComponent<ResultAnalysis>();

        c = board.transform.GetChild(2);
        enemyAnalysis = c.GetComponent<ResultAnalysis>();

        c = board.transform.GetChild(3);
        Button reStart = c.GetComponent<Button>();

        boardOpen.onClick.AddListener(board.Toggle);
        reStart.onClick.AddListener(ReStart);
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        user = gameManager.User;
        enemy = gameManager.Enemy;

        user.onActionEnd += () =>
        {
            userAnalysis.AllAttackCount = user.TotalAttackCount;
            userAnalysis.successAttackCount = user.SuccessAttackCount;
            userAnalysis.FailAttackCount = user.FailAttackCount;
            userAnalysis.SuccessAttackRate = (float)user.SuccessAttackCount / (float)user.TotalAttackCount;
        };

        enemy.onActionEnd += () =>
        {
            enemyAnalysis.AllAttackCount = enemy.TotalAttackCount;
            enemyAnalysis.successAttackCount = enemy.SuccessAttackCount;
            enemyAnalysis.FailAttackCount = enemy.FailAttackCount;
            enemyAnalysis.SuccessAttackRate = (float)enemy.SuccessAttackCount / (float)enemy.TotalAttackCount;
        };

        user.onDefeat += () => 
        {
            board.SetVictoryDefeat(false);
            Open();
        };
        enemy.onDefeat += () => 
        {
            board.SetVictoryDefeat(true);
            Open();
        };

        Close();
    }

    void Open()
    {
        gameObject.SetActive(true);
    }

    void Close() 
    {
        gameObject.SetActive(false);
    }

    void ReStart() 
    {
    
    }
}
