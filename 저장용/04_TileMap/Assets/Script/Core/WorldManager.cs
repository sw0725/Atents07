using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    const int HeightCount = 3;
    const int WidthCount = 3;
    const float mapHeightSize = 20.0f;
    const float mapWidthSize = 20.0f;
    const string sceneNameBase = "SceneLess_";
                                                             //���� ����(��� ���� ��ģ ���� ������ ���� �Ʒ��� ������)
    readonly Vector2 wordOrigine = new Vector2(-mapWidthSize * WidthCount* 0.5f, -mapHeightSize * HeightCount * 0.5f);

    string[] sceneNames;
    SceneLoadState[] sceneLoadState;

    List<int> loadWork = new List<int>();
    List<int> loadWorkComplete = new List<int>();
    List<int> unloadWork = new List<int>();
    List<int> unloadWorkComplete = new List<int>();

    enum SceneLoadState : byte 
    {
        Unload=0,
        PendingUnload,      //�ε����� ������
        PendingLoad,        //�ε� ������
        Load
    }

    public bool IsUnloadAll 
    {
        get 
        {
            bool result = true;
            foreach (SceneLoadState state in sceneLoadState) 
            {
                if (state != SceneLoadState.Unload) 
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }

    public void PreInitialize()      //ó�� ����������� �ѹ���
    {
        int mapCount = HeightCount * WidthCount;
        sceneNames = new string[mapCount];
        sceneLoadState = new SceneLoadState[mapCount];

        for (int y = 0; y < HeightCount; y++) 
        {
            for (int x = 0; x < WidthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{sceneNameBase}{x}{y}";
                sceneLoadState[index] = SceneLoadState.Unload;
            }
        }
    }

    public void Initialize()        //���� single�� �ε�ɶ����� ȣ��
    {
        for (int i = 0; i < sceneLoadState.Length; i++) 
        {
            sceneLoadState[i] = SceneLoadState.Unload;
        }

        Player player = GameManager.Instance.Player;
        if (player != null) 
        {
            player.onMapChange += (currentGrid) =>
            {
                RefreshScenes(currentGrid.x, currentGrid.y);
            };

            Vector2Int grid = WorldToGrid(player.transform.position);
            RequestAsyncSceneLoad(grid.x, grid.y);                      //�÷��̾ �ִ� ���� �ֿ켱 �ε�
            RefreshScenes(grid.x, grid.y);
        }
    }

    private int GetIndex(int x, int y)
    {
        return x + y * WidthCount;
    }

    void RequestAsyncSceneLoad(int x, int y) 
    {
        int index = GetIndex(x, y);
        if (sceneLoadState[index] == SceneLoadState.Unload) 
        {
            loadWork.Add(index);
        }
    }

    void AsyncSceneLoad(int index) 
    {
        if (sceneLoadState[index] == SceneLoadState.Unload) 
        {
            sceneLoadState[index] = SceneLoadState.PendingLoad;
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);
            async.completed += (_) =>                       //�񵿱� �۾��� ������ ����Ǵ� ��������Ʈ
            {
                sceneLoadState[index] = SceneLoadState.Load;
                loadWorkComplete.Add(index);
            };
        }
    }

    void RequestAsyncSceneUnload(int x, int y)
    {
        int index = GetIndex(x, y);
        if (sceneLoadState[index] == SceneLoadState.Load)
        {
            unloadWork.Add(index);
        }
    }

    void AsyncSceneUnload(int index)
    {
        if (sceneLoadState[index] == SceneLoadState.Load)
        {
            sceneLoadState[index] = SceneLoadState.PendingUnload;

            Scene scene = SceneManager.GetSceneByName(sceneNames[index]);       //�����ӵ��� ���� ��ε������ �� �����Ǵ°� ����
            if (scene.isLoaded)                                                 //�����ӵ��� Ǯ�� �ǵ�����.
            {                                   //�θ𰡾��� ���ӿ�����Ʈ ������
                GameObject[] sceneRoots = scene.GetRootGameObjects();
                if (sceneRoots != null && sceneRoots.Length > 0) 
                {
                    Slime[] slimes = sceneRoots[0].GetComponentsInChildren<Slime>();
                    foreach (Slime slime in slimes) 
                    {
                        slime.ReturnToPool();
                    }
                }
            }

            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index]);
            async.completed += (_) =>           //���������� +=�� ���� -=���ص� �ڿ��� �����
            {
                sceneLoadState[index] = SceneLoadState.Unload;
                unloadWorkComplete.Add(index);
            };
        }
    }

    private void Update()
    {   //�Ϸ�� �۾��� �ε������� ����
        foreach (var index in loadWorkComplete) 
        {                       //�ε��ũ���� �ε��ũ ���ø��� �ε����� ���� �������� ��� ����
            loadWork.RemoveAll((x)=>x==index);
        }
        loadWorkComplete.Clear();

        foreach (var index in loadWork) 
        {
            AsyncSceneLoad(index);
        }

        foreach (var index in unloadWorkComplete)
        {                       //�ε��ũ���� �ε����� ���� �������� ��� ����
            unloadWork.RemoveAll((x) => x == index);
        }
        unloadWorkComplete.Clear();

        foreach (var index in unloadWork)
        {
            AsyncSceneUnload(index);
        }
    }

    void RefreshScenes(int mapx, int mapy) //������ ��ġ �ֺ� �� �ε�, �� �� �ε� ����
    {
        int startx = Mathf.Max(0, mapx - 1);               //���� ū�Ͱ���
        int starty = Mathf.Max(0, mapy - 1);
        int endx = Mathf.Min(WidthCount, mapx + 2);         
        int endy = Mathf.Min(HeightCount, mapy + 2);

        List<Vector2Int> open = new List<Vector2Int>(9);

        for (int y = starty; y < endy; y++) //���ϴ�(����)���� ���ʷ� 9ĭ Ž��
        {
            for (int x = startx; x < endx; x++)
            {
                RequestAsyncSceneLoad(x, y);
                open.Add(new Vector2Int(x,y));
            }
        }

        for (int y = 0; y < HeightCount; y++)
        {
            for (int x = 0; x < WidthCount; x++)
            {           //contains �ش� �������� �ִ°� //exits ������ �����ϴ°��� �ִ°�
                if (!open.Contains(new Vector2Int(x, y)))
                {
                    RequestAsyncSceneUnload(x, y);
                }
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition) //���� ��ǥ�� 0,0-2,2
    {
        Vector2 offset = (Vector2)worldPosition - wordOrigine;
        return new Vector2Int((int)(offset.x/mapWidthSize), (int)(offset.y/mapHeightSize));
    }

#if UNITY_EDITOR
    public void TestLoadScene(int x, int y) 
    {
        RequestAsyncSceneLoad(x, y);
    }

    public void TestUnloadScene(int x, int y)
    {
        RequestAsyncSceneUnload(x, y);
    }

    public void TestRefreshScene(int x, int y) 
    {
        RefreshScenes(x, y);
    }

    public void TestUnloadAllScene() 
    {
        for (int y = 0; y < HeightCount; y++)
        {
            for (int x = 0; x < WidthCount; x++)
            {
                RequestAsyncSceneUnload(x, y);
            }
        }
    }
#endif
}
