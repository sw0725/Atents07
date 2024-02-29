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
                                                             //맵의 원점(모든 맵을 합친 맵의 원점이 왼쪽 아래로 가도록)
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
        PendingUnload,      //로드해제 진행중
        PendingLoad,        //로딩 진행중
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

    public void PreInitialize()      //처음 만들어졌을때 한번만
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

    public void Initialize()        //씬이 single로 로드될때마다 호출
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
            RequestAsyncSceneLoad(grid.x, grid.y);                      //플레이어가 있는 맵을 최우선 로딩
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
            async.completed += (_) =>                       //비동기 작업이 끝날시 실행되는 델리게이트
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

            Scene scene = SceneManager.GetSceneByName(sceneNames[index]);       //슬라임들이 씬이 언로드됬을때 걍 삭제되는것 방지
            if (scene.isLoaded)                                                 //슬라임들을 풀로 되돌린다.
            {                                   //부모가없는 게임오브젝트 가져옴
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
            async.completed += (_) =>           //지역변수라 +=을 도로 -=안해도 자연히 사라짐
            {
                sceneLoadState[index] = SceneLoadState.Unload;
                unloadWorkComplete.Add(index);
            };
        }
    }

    private void Update()
    {   //완료된 작업을 인덱스에서 제거
        foreach (var index in loadWorkComplete) 
        {                       //로드워크에서 로드워크 컴플릿의 인덱스와 같은 아이템을 모두 제거
            loadWork.RemoveAll((x)=>x==index);
        }
        loadWorkComplete.Clear();

        foreach (var index in loadWork) 
        {
            AsyncSceneLoad(index);
        }

        foreach (var index in unloadWorkComplete)
        {                       //로드워크에서 인덱스와 같은 아이템을 모두 제거
            unloadWork.RemoveAll((x) => x == index);
        }
        unloadWorkComplete.Clear();

        foreach (var index in unloadWork)
        {
            AsyncSceneUnload(index);
        }
    }

    void RefreshScenes(int mapx, int mapy) //지정된 위치 주변 맵 로딩, 그 외 로딩 해제
    {
        int startx = Mathf.Max(0, mapx - 1);               //둘중 큰것고르기
        int starty = Mathf.Max(0, mapy - 1);
        int endx = Mathf.Min(WidthCount, mapx + 2);         
        int endy = Mathf.Min(HeightCount, mapy + 2);

        List<Vector2Int> open = new List<Vector2Int>(9);

        for (int y = starty; y < endy; y++) //좌하단(원점)부터 차례로 9칸 탐색
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
            {           //contains 해당 아이템이 있는가 //exits 조건을 만족하는것이 있는가
                if (!open.Contains(new Vector2Int(x, y)))
                {
                    RequestAsyncSceneUnload(x, y);
                }
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition) //맵의 좌표는 0,0-2,2
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
