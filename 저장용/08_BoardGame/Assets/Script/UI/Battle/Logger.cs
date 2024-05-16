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
    const string YOU = "���";
    const string ENEMY = "��";

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
            ship.onHit += (target) => AttackSuccess(false, target);                         //�����Լ� ����
            ship.onSink = (target) => { ShipSinking(false, target); } + ship.onSink;        //��������Ʈ�� �۵��Ǵ� �Լ��� ���� ����
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

        TurnStart(1);       //���������� ����
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
        Log($"<b><#{colortext}>{number}</color></b> ° ȸ���� ���۵Ǿ����ϴ�.");
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

        Log($"<b><#{attackerColor}>{attackerName}</color></b>�� ����\t : <b><#{hitterColor}>{hitterName}</color></b>�� <b><#{shipTextColor}>{ship.name}</color></b>�� �����߽��ϴ�");
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

        Log($"<b><#{attackerColor}>{attackerName}</color></b>�� ����\t : <b><#{attackerColor}>{attackerName}</color></b>�� ��ź�� ���������ϴ�");
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

        Log($"<b><#{attackerColor}>{attackerName}</color></b>�� ����\t : <b><#{hitterColor}>{hitterName}</color></b>�� <b><#{shipTextColor}>{ship.name}</color></b>�� ħ���߽��ϴ�");
    }

    void Defeat(bool isUser)
    {
        string temp = $"<b><#{ColorUtility.ToHtmlStringRGB(userColor)}>{YOU}</color></b>��";
        if (isUser) 
        {
            Log($"{temp} �й�...");
        }
        else
        {
            Log($"{temp} �¸�!!!");
        }
    }
}
