using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarBase : MonoBehaviour
{
    public Color color = Color.white;

    protected Slider slider;
    protected TextMeshProUGUI current;
    protected TextMeshProUGUI max;

    protected float maxValue;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        Transform c = transform.GetChild(2);
        current = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(3);
        max = c.GetComponent<TextMeshProUGUI>();

        c = transform.GetChild(0);
        Image bgImage = c.GetComponent<Image>();
        bgImage.color = new (color.r, color.g, color.b, color.a * 0.5f);

        c = transform.GetChild(1);
        Image fillImage = c.GetComponentInChildren<Image>();
        fillImage.color = color;
    }

    protected void OnValueChange(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        slider.value = ratio;
        current.text = $"{(ratio * maxValue):f0}";     //소수점 버리기
    }
}
