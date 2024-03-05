using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImage;

    Image[] digits;

    int number = 0;

    public int Number 
    {
        get => number;
        set 
        {
            if(number != value) 
            {
                number = value;

                if (number % 10 > 0)
                {
                    digits[0].sprite = numberImage[number % 10];
                    digits[0].gameObject.SetActive(true);
                }
                else 
                {
                    digits[0].gameObject.SetActive(false);
                }
                if ((number / 10) % 10 > 0)
                {
                    digits[1].sprite = numberImage[(number / 10) % 10];
                    digits[0].gameObject.SetActive(true); digits[1].gameObject.SetActive(true);
                }
                else
                {
                    digits[1].gameObject.SetActive(false);
                }
                if ((number / 100) % 10 > 0)
                {
                    digits[2].sprite = numberImage[(number / 100) % 10];
                    digits[0].gameObject.SetActive(true); digits[1].gameObject.SetActive(true); digits[2].gameObject.SetActive(true);
                }
                else
                {
                    digits[2].gameObject.SetActive(false);
                }
                if ((number / 1000) % 10 > 0)
                {
                    digits[3].sprite = numberImage[(number / 1000) % 10];
                    digits[0].gameObject.SetActive(true); digits[1].gameObject.SetActive(true); digits[2].gameObject.SetActive(true); digits[3].gameObject.SetActive(true);
                }
                else
                {
                    digits[3].gameObject.SetActive(false);
                }
                if ((number / 10000) % 10 > 0)
                {
                    digits[4].sprite = numberImage[(number / 10000) % 10];
                    digits[0].gameObject.SetActive(true); digits[1].gameObject.SetActive(true); digits[2].gameObject.SetActive(true); digits[3].gameObject.SetActive(true); digits[4].gameObject.SetActive(true);
                }
                else
                {
                    digits[4].gameObject.SetActive(false);
                }
            }
        }
    }

    private void Awake()
    {
        digits = GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onKillCountChange += KillCountChange;
    }

    private void KillCountChange(int killcount)
    {
        Number = killcount;
    }
}
