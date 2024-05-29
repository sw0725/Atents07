using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(MazeVisualizer))]
[RequireComponent(typeof(NavMeshSurface))]
public class MazeGenerator : MonoBehaviour
{
    public enum MazeAlgorithm 
    {
        RecursiveBackTracking = 0,
        Eller,
        Wilson
    }
    public MazeAlgorithm mazeAlgorithm = MazeAlgorithm.Wilson;
    public int seed = -1;

    MazeVisualizer visualizer;
    NavMeshSurface surface;              //�׺�޽� ������ �����ɸ��ϱ� �񵿱����� ��
    AsyncOperation async;

    private void Awake()
    {
        visualizer = GetComponent<MazeVisualizer>();
        surface = GetComponent<NavMeshSurface>();
    }

    public void Generate(int width, int height, MazeAlgorithm algorithm = MazeAlgorithm.Wilson) 
    {
        Maze maze = null;
        switch (algorithm) 
        {
            case MazeAlgorithm.RecursiveBackTracking:
                maze = new BackTracking();
                break;
            case MazeAlgorithm.Eller:
                maze = new Eller();
                break;
            case MazeAlgorithm.Wilson:
                maze = new Wilson();
                break;
        }
        maze.MakeMaze(width, height, seed);

        visualizer.Clear();
        visualizer.Draw(maze);
        StartCoroutine(UpdateSurface());
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
