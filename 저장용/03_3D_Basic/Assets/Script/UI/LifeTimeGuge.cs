using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGuge : MonoBehaviour
{
    Slider silder;
    TextMeshProUGUI textMeshPro;

    float maxValue = 0.0f;

    private void Awake()
    {
        silder = GetComponent<Slider>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.OnLifeTimeChange += Refresh;
        player.OnDie += Stop;
        maxValue = player.startLiftime;
    }

    private void Refresh(float ratio)
    {
        silder.value = ratio;
        textMeshPro.text = $"{(ratio * maxValue):f1}sec";           //:f1 => 소수점 한자리만 출력
    }

    private void Stop()
    {
        Player player = GameManager.Instance.Player;
        player.OnLifeTimeChange -= Refresh;
        player.OnDie -= Stop;
    }
}
