using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    TextMeshProUGUI title;
    TextMeshProUGUI kill;
    TextMeshProUGUI time;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        title = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(2);

        kill = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(4);

        time = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(5);

        Button restart = c.GetComponent<Button>();
        restart.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    public void Open(bool isClear, int killCount, float playTime) 
    {
        if (isClear)
        {
            title.text = "Game Clear";
        }
        else
        {
            title.text = "Game Over";
        }
        kill.text = killCount.ToString();
        time.text = playTime.ToString("f1");
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }
}
