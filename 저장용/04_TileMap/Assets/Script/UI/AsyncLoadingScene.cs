using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    public string nextSceneName = "LoadSampleScene";        //�ε������� �ҷ��� �� �̸�
    public float loadindBarSpeed = 1.0f;

    float loadRatio;        //�����̴��� ���� ������ ��
    bool loaingDone = false;
    IEnumerator loadingTextCoroutine;       //���ں���� �ڷ�ƾ

    AsyncOperation async;       //�񵿱� ��� ó���� ���� �ʿ��� Ŭ����

    Slider lodingSlider;
    TextMeshProUGUI loadingText;
    PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();

        inputActions.UI.Click.performed += Press;
        inputActions.UI.AnyKey.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.Click.performed -= Press;
        inputActions.UI.AnyKey.performed -= Press;

        inputActions.UI.Disable();
    }

    private void Start()
    {
        lodingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();
        loadingTextCoroutine = LoadingTextProgress();

        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(AsyncLoadScene());
    }

    private void Update()
    {
        if (lodingSlider.value < loadRatio) 
        {
            lodingSlider.value += Time.deltaTime * loadindBarSpeed; 
        }
    }

    private void Press(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = loaingDone;
    }

    IEnumerator LoadingTextProgress() 
    {//0.2���� .��, .�� �ִ� 5��
        loadingText.text = "Loading";
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        string[] text = { "Loading", "Loading.", "Loading..", "Loading...", "Loading....", "Loading....." };
        int index=0;
        while (true) 
        {
            loadingText.text = text[index];
            index++;
            index %= text.Length;
            yield return wait;
        }
    }

    IEnumerator AsyncLoadScene() 
    {
        loadRatio = 0.0f;
        lodingSlider.value = 0.0f;

        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;
        while (loadRatio < 1.0f) 
        {
            loadRatio = async.progress + 0.1f;
            yield return null;
        }
                                        //���� ���� �����µ� �ɸ��� �ð�: ������/���� �ӵ�
        yield return new WaitForSeconds((1-lodingSlider.value)/loadindBarSpeed); ;

        StopCoroutine(loadingTextCoroutine);
        loaingDone = true;
        loadingText.text = "Loading\nComplete!";
    }
}
