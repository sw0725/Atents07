using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HitDirection : MonoBehaviour
{
    public float duration = 0.5f;

    float reverseDuration;
    float timeelapsed = 0.0f;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        reverseDuration = 1/ duration;

        GameManager.Instance.Player.onAttacked += OnPlayerAttacked;
        timeelapsed = duration;
        image.color = Color.clear;
    }

    private void Update()
    {
        timeelapsed += Time.deltaTime;
        float alpha = timeelapsed * reverseDuration;
        image.color = Color.Lerp(Color.white, Color.clear, alpha);
    }

    void OnPlayerAttacked(float angle) 
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        image.color = Color.white;
        timeelapsed = 0;
    }
}
