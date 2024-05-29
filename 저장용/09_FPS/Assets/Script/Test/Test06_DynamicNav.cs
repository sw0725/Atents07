using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test06_DynamicNav : TestBase
{
    [Header("미로")]
    public MazeVisualizer backTracking;
    public MazeVisualizer eller;
    public MazeVisualizer wilson;

    public int width = 5;
    public int height = 5;

    public MazeGenerator generator;
    public NavMeshSurface surface;              //네브메쉬 생성은 오래걸리니까 비동기방식이 굿
    AsyncOperation async;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        backTracking.Clear();

        BackTracking maze1 = new BackTracking();
        maze1.MakeMaze(width, height, seed);
        backTracking.Draw(maze1);

        eller.Clear();

        Eller maze2 = new Eller();
        maze2.MakeMaze(width, height, seed);
        eller.Draw(maze2);

        wilson.Clear();

        Wilson maze3 = new Wilson();
        maze3.MakeMaze(width, height, seed);
        wilson.Draw(maze3);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        StartCoroutine(UpdateSurface());
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        generator.Generate(width, height);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
    }

    IEnumerator UpdateSurface() 
    {
        async = surface.UpdateNavMesh(surface.navMeshData);
        while (!async.isDone) 
        {
            yield return null;
        }
    }
}
