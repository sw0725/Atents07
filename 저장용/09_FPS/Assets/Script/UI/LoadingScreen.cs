using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public float Progress 
    {
        get => progress;
        set 
        {
            progress = Mathf.Min(targetProgress, value);
            slider.value = progress;

            if(progress > 0.99f) 
            {
                OnLoadingComplete();
            }
        }
    }
    float progress = 0.0f;
    float targetProgress = 0.0f;

    string[] loadingStrings = { "Loading .", "Loading . .", "Loading . . ." };

    Slider slider;
    TextMeshProUGUI loadingText;
    TextMeshProUGUI completeText;
    TextMeshProUGUI pressText;

    PlayerInputAction action;

    private void Awake()
    {
        action = new PlayerInputAction();

        Transform c = transform.GetChild(0);
        loadingText = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(1);
        completeText = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(2);
        pressText = c.GetComponent<TextMeshProUGUI>();

        c = transform.GetChild(3);
        slider = c.GetComponent<Slider>();
        slider.value = 0;
    }

    private void OnEnable()
    {
        action.UI.AnyKey.performed += OnAnyKey;
    }

    private void OnDisable()
    {
        action.UI.AnyKey.performed -= OnAnyKey;
        action.UI.Disable();
    }

    private void Update()
    {
        Progress += Time.deltaTime;
    }

    public void Initialize()
    {
        Progress = 0.0f;
        targetProgress = 0.5f;
        StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine() 
    {
        int index = 0;
        while (true) 
        {
            loadingText.text = loadingStrings[index];
            index = (index + 1) % loadingStrings.Length;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OnLoadingProgress(float progress) 
    {
        targetProgress = progress;
    }

    void OnLoadingComplete() 
    {
        loadingText.gameObject.SetActive(false);
        completeText.gameObject.SetActive(true);
        pressText.gameObject.SetActive(true);

        slider.value = 1;

        StopAllCoroutines();

        action.UI.Enable();
    }

    private void OnAnyKey(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameStart();
    }
}
