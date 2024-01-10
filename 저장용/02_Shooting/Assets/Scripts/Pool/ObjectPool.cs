using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : RecycleObject
{
    public GameObject originalPrefab;

    public int poolSize = 64;

    T[] Pool;

    Queue<T> Queue;

    private void Awake()
    {
        Initialized();
    }

    public void Initialized()
    {
        if (Pool == null)
        {
            Pool = new T[poolSize];
            Queue = new Queue<T>(poolSize);

            GenerateObject(0, poolSize, Pool);
        }
        else 
        {
            foreach (T obj in Pool) 
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    private void GenerateObject(int start, int end, T[] result)
    {
        for (int i = 0; i<end; i++) 
        {
            GameObject obj = Instantiate(originalPrefab, transform);
            obj.name = $"{originalPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();
            comp.onDisable += () => Queue.Enqueue(comp);
            Queue.Enqueue(comp);

            result[i] = comp;
            obj.SetActive(false);
        }
    }

    public T GetObject() 
    {
        if (Queue.Count > 0)
        {
            T comp = Queue.Dequeue();
            comp.gameObject.SetActive(true);
            return comp;
        }
        else 
        {
            ExpendPool();
            return GetObject();
        }
    }

    void ExpendPool() 
    {
        Debug.LogWarning($"{gameObject.name} 풀 사이즈 증가 {poolSize*2}");
        int newSize = poolSize * 2;
        T[] newPool = new T[newSize];
        for (int i= 0; i < poolSize; i++) 
        {
            newPool[i] = Pool[i];
        }
        GenerateObject(poolSize, newSize, newPool);

        poolSize = newSize;
        Pool = newPool;
    }
}
