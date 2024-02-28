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
    public string nextSceneName = "LoadSampleScene";        //로딩씬이후 불려질 씬 이름
    public float loadindBarSpeed = 1.0f;

    float loadRatio;        //슬라이더의 값에 영향을 줌
    bool loaingDone = false;
    IEnumerator loadingTextCoroutine;       //글자변경용 코루틴

    AsyncOperation async;       //비동기 명령 처리를 위해 필요한 클래스

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
    {//0.2간격 .찍, .은 최대 5개
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
                                        //남은 양이 다차는데 걸리는 시간: 남은양/차는 속도
        yield return new WaitForSeconds((1-lodingSlider.value)/loadindBarSpeed); ;

        StopCoroutine(loadingTextCoroutine);
        loaingDone = true;
        loadingText.text = "Loading\nComplete!";
    }
}
