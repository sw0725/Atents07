using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_AsyncLoad : TestBase
{
    public string nextSceneName = "LoadSampleScene";
    AsyncOperation async;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;     
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true;      //씬로딩 완료시 자동 씬 전환
    }

    IEnumerator LoadSceneCoroutine()                            //코루틴으로 로딩중 다른 행동을 병행할 수 있도록 함
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);     //씬로드가 끋나면 바로 씬으로 넘어간다.
        async.allowSceneActivation = false;                     //로 로딩이 끝나도 자동으로 다음씬으로 넘어가지 못하도록 설정

        while (async.progress < 0.9)    //allowSceneActivation설정시 0.9가 최대
        {
            Debug.Log($"Progress : {async.progress}");
            yield return null;
        }
        Debug.Log("Loading Complete");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        StartCoroutine(LoadSceneCoroutine());
    }
}
