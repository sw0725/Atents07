using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeText : MonoBehaviour
{
    TextMeshProUGUI timeText;
    float maxLife;

    private void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        maxLife = player.maxLifeTime;
        player.onLifeTimeChange += LifeTimeChage;
        timeText.text = $"{maxLife:f2} sec";
    }

    void LifeTimeChage(float ratio) 
    {
        timeText.text = $"{(maxLife * ratio):f2} sec";
    }
}
