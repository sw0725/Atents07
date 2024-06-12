using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodOverLay : MonoBehaviour
{
    public AnimationCurve curve;
    public Color color = Color.clear;

    float reverseMaxHP;

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

    void OnHPChage(float health) 
    {
        color.a = curve.Evaluate(1 - (health * reverseMaxHP));
        image.color = color;
    }
}
