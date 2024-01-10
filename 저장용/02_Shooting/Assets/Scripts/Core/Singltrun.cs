using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singltrun : MonoBehaviour//해당 클래스는 new생성 불가능->생성자 신경쓰지 않아도 됨
{
    bool isinitialize;
    static bool isShutdown = false;
    private static Singltrun instance = null;
    public static Singltrun Instance
    {
        get
        {
            if (isShutdown) 
            {
                return null;
            }
            if (instance == null)
            {
                Singltrun s = FindAnyObjectByType<Singltrun>();
                if (s == null)
                {
                    GameObject dbj = new GameObject();
                    dbj.name = "Sington";
                    instance = dbj.AddComponent<Singltrun>();//모노에 뉴는 불가하니까 이런식으로 대신함
                }
                instance = s;
                DontDestroyOnLoad(instance.gameObject);//씬이 사라져도 오브젝트 삭제 안됨
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            if (instance != this) { Destroy(this); }
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
        if (!isinitialize) { OnpreInitialize(); }
        if (mode != LoadSceneMode.Additive) { OnInitialize(); }
    }

    protected virtual void OnpreInitialize() 
    {
    
    }
    protected virtual void OnInitialize()
    {

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
