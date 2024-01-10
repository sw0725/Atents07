using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singltrun<T> : MonoBehaviour where T : Component
{
    bool isinitialize;
    static bool isShutdown = false;
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (isShutdown) 
            {
                Debug.LogWarning("싱글톤은 이미 삭제중이다.");
                return null;
            }
            if (instance == null)
            {
                T singleton = FindAnyObjectByType<T>();
                if (singleton == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "Sington";
                    singleton = obj.AddComponent<T>();
                }
                instance = singleton;
                DontDestroyOnLoad(instance.gameObject);//씬이 사라져도 오브젝트 삭제 안됨
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            if (instance != this) 
            {
                Destroy(this.gameObject); 
            }
        }
    }
    private void OnApplicationQuit()
    {
        isShutdown = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    private void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        if (!isinitialize) 
        {
            OnpreInitialize();
        }
        if (mode != LoadSceneMode.Additive) 
        {
            OnInitialize(); 
        }
    }

    protected virtual void OnpreInitialize() 
    {
    
    }
    protected virtual void OnInitialize()
    {
        isinitialize = true;
    }
}
//반드시 한개의 객체
public class TestSingleton 
{
    public int indexer;

    private static TestSingleton single = null;//싱글턴 저장자리 클래스 밖에서 접근할수 잇어야가지고 스테틱임

    public static TestSingleton Single //만들때 인스턴스 있나없나 검사
    {
        get
        {
            if (single == null)
            {
                single = new TestSingleton();
            }
            return single;
        }
    }
    private TestSingleton() 
    {
        //생성자에private-> 객체 중복생성 불가 = 싱글턴 완성!
    }
}
