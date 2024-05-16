using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;

    TextMeshProUGUI log;
    StringBuilder builder;

    List<string> Lines;

    const int MaxLineCount = 20;
    const string YOU = "당신";
    const string ENEMY = "적";

    private void Awake()
    {
        log = GetComponentInChildren<TextMeshProUGUI>();
        Lines = new List<string>(MaxLineCount +1);
        builder = new StringBuilder(MaxLineCount +1);
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.TurnManager.onTurnStart += TurnStart;

        UserPlayer user = gameManager.User;
        EnemyPlayer enemy = gameManager.Enemy;
        foreach(Ship ship in user.Ships) 
        {
            ship.onHit += (target) => AttackSuccess(false, target);                         //유저함선 적중
            ship.onSink = (target) => { ShipSinking(false, target); } + ship.onSink;        //델리게이트로 작동되는 함수들 순서 정리
        }
        foreach (Ship ship in enemy.Ships)
        {
            ship.onHit += (target) => AttackSuccess(true, target);
            ship.onSink = (target) => { ShipSinking(true, target); } + ship.onSink;
        }
        user.onAttackFail += AttackFail;
        enemy.onAttackFail += AttackFail;

        user.onDefeat += ()=> Defeat(true);
        enemy.onDefeat += ()=> Defeat(false);

        Clear();

        TurnStart(1);       //서순문제로 인함
    }

    void Log(string text) 
    {
        Lines.Add(text);
        if (Lines.Count > MaxLineCount) 
        {
            Lines.RemoveAt(0);
        }

        builder.Clear();
        foreach (string line in Lines) 
        {
            builder.AppendLine(line);
        }

        log.text = builder.ToString();
    }

    void Clear() 
    {
        Lines.Clear();
        log.text = string.Empty;
    }

    void TurnStart(int number) 
    {
        string colortext = ColorUtility.ToHtmlStringRGB(userColor);
        Log($"<b><#{colortext}>{number}</color></b> 째 회차가 시작되었습니다.");
    }

    void AttackSuccess(bool isUser, Ship ship) 
    {
        string attackerName;
        string attackerColor;
        string hitterName;
        string hitterColor;
        string shipTextColor;
        if (isUser) 
        {
            attackerName = YOU;
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            hitterName = ENEMY;
            hitterColor = ColorUtility.ToHtmlStringRGB(enemyColor);
        }
        else
        {
            attackerName = ENEMY;
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            hitterName = YOU;
            hitterColor = ColorUtility.ToHtmlStringRGB(userColor);
        }
        shipTextColor = ColorUtility.ToHtmlStringRGB(shipColor);

        Log($"<b><#{attackerColor}>{attackerName}</color></b>의 공격\t : <b><#{hitterColor}>{hitterName}</color></b>의 <b><#{shipTextColor}>{ship.name}</color></b>에 명중했습니다");
    }

    void AttackFail(bool isUser)
    {
        string attackerName;
        string attackerColor;
        if (isUser)
        {
            attackerName = YOU;
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
        }
        else
        {
            attackerName = ENEMY;
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
        }

        Log($"<b><#{attackerColor}>{attackerName}</color></b>의 공격\t : <b><#{attackerColor}>{attackerName}</color></b>의 포탄이 빗나갔습니다");
    }

    void ShipSinking(bool isUser, Ship ship) 
    {
        string attackerName;
        string attackerColor;
        string hitterName;
        string hitterColor;
        string shipTextColor;
        if (isUser)
        {
            attackerName = YOU;
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            hitterName = ENEMY;
            hitterColor = ColorUtility.ToHtmlStringRGB(enemyColor);
        }
        else
        {
            attackerName = ENEMY;
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            hitterName = YOU;
            hitterColor = ColorUtility.ToHtmlStringRGB(userColor);
        }
        shipTextColor = ColorUtility.ToHtmlStringRGB(shipColor);

        Log($"<b><#{attackerColor}>{attackerName}</color></b>의 공격\t : <b><#{hitterColor}>{hitterName}</color></b>의 <b><#{shipTextColor}>{ship.name}</color></b>가 침몰했습니다");
    }

    void Defeat(bool isUser)
    {
        string temp = $"<b><#{ColorUtility.ToHtmlStringRGB(userColor)}>{YOU}</color></b>의";
        if (isUser) 
        {
            Log($"{temp} 패배...");
        }
        else
        {
            Log($"{temp} 승리!!!");
        }
    }
}
