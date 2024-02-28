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
        async.allowSceneActivation = true;      //���ε� �Ϸ�� �ڵ� �� ��ȯ
    }

    IEnumerator LoadSceneCoroutine()                            //�ڷ�ƾ���� �ε��� �ٸ� �ൿ�� ������ �� �ֵ��� ��
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);     //���ε尡 ������ �ٷ� ������ �Ѿ��.
        async.allowSceneActivation = false;                     //�� �ε��� ������ �ڵ����� ���������� �Ѿ�� ���ϵ��� ����

        while (async.progress < 0.9)    //allowSceneActivation������ 0.9�� �ִ�
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
