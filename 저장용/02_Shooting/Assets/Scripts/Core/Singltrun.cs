using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singltrun : MonoBehaviour//�ش� Ŭ������ new���� �Ұ���->������ �Ű澲�� �ʾƵ� ��
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
                    instance = dbj.AddComponent<Singltrun>();//��뿡 ���� �Ұ��ϴϱ� �̷������� �����
                }
                instance = s;
                DontDestroyOnLoad(instance.gameObject);//���� ������� ������Ʈ ���� �ȵ�
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
//�ݵ�� �Ѱ��� ��ü
public class TestSingleton 
{
    public int indexer;

    private static TestSingleton single = null;//�̱��� �����ڸ� Ŭ���� �ۿ��� �����Ҽ� �վ�߰����� ����ƽ��

    public static TestSingleton Single //���鶧 �ν��Ͻ� �ֳ����� �˻�
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
        //�����ڿ�private-> ��ü �ߺ����� �Ұ� = �̱��� �ϼ�!
    }
}
