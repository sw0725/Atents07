using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour
{
    public GameObject originalPrefab;

    public int poolSize = 64;

    T[] Pool;

    Queue<T> Queue;

    public void Initialized()
    {
        Pool = new T[poolSize];
        Queue = new Queue<T>(poolSize);

        GenerateObject(0, poolSize, Pool);
    }

    private void GenerateObject(int start, int end, T[] result)
    {
        for (int i = 0; i<end; i++) 
        {
            GameObject obj = Instantiate(originalPrefab, transform);
            obj.name = $"{originalPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();

            result[i] = comp;
            obj.SetActive(false);
        }
    }
}
