using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    Slider slider;
    Image fill;

    public Gradient Gcolor;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;

        Transform c = transform.GetChild(1);
        fill = c.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onLifeTimeChange += LifeTimeChage;
    }

    void LifeTimeChage(float ratio) 
    {
        slider.value = ratio;

        fill.color = Gcolor.Evaluate(ratio);
    }
}
