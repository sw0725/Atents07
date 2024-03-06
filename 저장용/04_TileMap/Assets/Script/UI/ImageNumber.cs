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

    int number = -1;

    public int Number 
    {
        get => number;
        set 
        {
            if (number != value)
            {
                number = Mathf.Min(value, 99999);
                int num = number;
                for (int i = 0; i < digits.Length; i++) 
                {
                    if (num != 0 || i ==0)
                    {
                        int digit = num % 10;
                        digits[i].sprite = numberImage[digit];
                        digits[i].gameObject.SetActive(true);
                    }
                    else 
                    {
                        digits[i].gameObject.SetActive(false);
                    }
                    num /= 10;
                }
            }
        }
    }

    private void Awake()
    {
        digits = GetComponentsInChildren<Image>();
    }
}
