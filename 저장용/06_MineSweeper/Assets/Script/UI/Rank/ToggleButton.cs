using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    public Action<bool> onToggleChange;

    bool isOn = false;

    Button button;
    Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(() => SetToggleState(!isOn));
    }

    private void Start()
    {
        isOn = true;
        SetToggleState(isOn);
    }

    void SetToggleState(bool on) 
    {
        isOn = on;
        if (isOn)
        {
            image.sprite = onSprite;
        }
        else 
        {
            image.sprite = offSprite;
        }
        onToggleChange?.Invoke(isOn);
    }

    public void ToggleOn() 
    {
        SetToggleState(true);
    }
}
