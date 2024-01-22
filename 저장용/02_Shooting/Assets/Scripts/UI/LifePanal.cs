using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePanal : MonoBehaviour
{
    public Color diablecolor;

    Image[] lifeImage;

    private void Awake()
    {
        lifeImage = new Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) 
        {
            Transform child = transform.GetChild(i);
            lifeImage[i] = child.GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        Player player = GameManager.Instance.Player;
        if (player != null) 
        {
            player.onLifeChange += OnLifeChange;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null) 
        {
            Player player = GameManager.Instance.Player;
            player.onLifeChange -= OnLifeChange;
        }
    }

    private void OnLifeChange(int obj) //n->n-1->n-2..3->2->1->0
    {
        for (int i = 0; i < obj; i++) 
        {
            lifeImage[i].color = Color.white;
        }
        for (int i = obj; i < lifeImage.Length; i++) 
        {
            lifeImage[i].color = diablecolor;
        }
    }
}
