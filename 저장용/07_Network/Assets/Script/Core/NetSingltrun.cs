using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetSingltrun<T> : NetworkBehaviour where T : Component
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
