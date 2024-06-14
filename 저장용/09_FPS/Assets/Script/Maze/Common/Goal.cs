using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Goal : MonoBehaviour
{
    public System.Action onGameClear;

    public void SetRandomPos(int width, int height) 
    {
        Vector2Int result = new Vector2Int();

        int dir = Random.Range(0, 4);       //ºÏ µ¿ ³² ¼­
        switch (dir)
        {
            case 0:
                result.x = Random.Range(0, width);
                result.y = 0;
                break;
            case 1:
                result.x = width - 1;
                result.y = Random.Range(0, height);
                break;
            case 2:
                result.x = Random.Range(0, width);
                result.y = height - 1;
                break;
            case 3:
                result.x = 0;
                result.y = Random.Range(0, height);
                break;
        }
        transform.position = MazeVisualizer.GridToWorld(result.x, result.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            onGameClear?.Invoke();
        }
    }

#if UNITY_EDITOR
    public Vector2Int TestSetRandomPos(int width, int height) 
    {
        Vector2Int result = new Vector2Int();

        int dir = Random.Range(0, 4);       //ºÏ µ¿ ³² ¼­
        switch(dir) 
        {
            case 0:
                result.x = Random.Range(0, width);
                result.y = 0;
                break;
            case 1:
                result.x = width - 1;
                result.y = Random.Range(0, height);
                break;
            case 2:
                result.x = Random.Range(0, width);
                result.y = height - 1;
                break;
            case 3:
                result.x = 0;
                result.y = Random.Range(0, height);
                break;
        }

        return result;
    }
#endif
}
