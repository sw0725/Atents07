using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    TextMeshProUGUI playtime;
    TextMeshProUGUI killCount;
    CanvasGroup canvasGroup;
    Button reStart;

    public float alphaChangeSpeed = 1.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform c = transform.GetChild(1);
        playtime = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(2);
        killCount = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(3);
        reStart = c.GetComponent<Button>();

        reStart.onClick.AddListener(() => 
        {
            StartCoroutine(WaitUnloadAll());
        });
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Player player = GameManager.Instance.Player;
        player.onDie += PlayerDie;
    }

    private void PlayerDie(float totalPlayTime, int totalKillCount)
    {
        playtime.text = $"Total Play Time\n\r< {totalPlayTime:f1} Sec >";
        killCount.text = $"Total Kill Count\n\r< {totalKillCount} Kill >";
        StartCoroutine(StartAlphaCahnge());
    }

    IEnumerator StartAlphaCahnge()
    {
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator WaitUnloadAll()
    {
        WorldManager world = GameManager.Instance.World;
        while (!world.IsUnloadAll) //�÷��̾� �װ� �ε� ���� �� ������ ���
        {
            yield return null;
        }
        SceneManager.LoadScene("AsyncLoadScene");
    }
}
