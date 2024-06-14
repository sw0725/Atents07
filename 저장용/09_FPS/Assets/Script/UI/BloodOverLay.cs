using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodOverLay : MonoBehaviour
{
    public AnimationCurve curve;
    public Color color = Color.clear;

    float reverseMaxHP;
    float targetAlpha = 0;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = color;
    }

    private void Start()
    {
        GameManager.Instance.Player.onHPChange += OnHPChage;
        reverseMaxHP = 1 / GameManager.Instance.Player.MaxHP;
    }

    private void Update()
    {
        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime);
        image.color = color;
    }

    void OnHPChage(float health) 
    {
        targetAlpha = curve.Evaluate(1 - (health * reverseMaxHP));
    }
}
