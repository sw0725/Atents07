using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Info : MonoBehaviour
{
    TextMeshProUGUI action;
    TextMeshProUGUI find;
    TextMeshProUGUI notFind;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        c = c.GetChild(1);
        action = c.GetComponent<TextMeshProUGUI>();

        c = transform.GetChild(1);
        c = c.GetChild(1);
        find = c.GetComponent<TextMeshProUGUI>();

        c = transform.GetChild(2);
        c = c.GetChild(1);
        notFind = c.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.onGameReady += onGameReady;
        gameManager.onActionCountChange += onActionCountChange;
        gameManager.onGameClear += onGameEnd;
        gameManager.onGameOver += onGameEnd;
    }

    private void onGameEnd()
    {
        int found = GameManager.Instance.Board.FoundMineCount;
        int notFound = GameManager.Instance.Board.NotFoundMineCount;

        find.text = found.ToString();
        notFind.text = notFound.ToString();
    }

    private void onGameReady()
    {
        action.text = "???";
        find.text = "???";
        notFind.text = "???";
    }

    private void onActionCountChange(int count)
    {
        action.text = count.ToString();
    }
}
