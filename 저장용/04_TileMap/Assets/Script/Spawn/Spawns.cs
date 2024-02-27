using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spawns : MonoBehaviour
{
    public float interval = 1.0f;
    public Vector2 size;            //���� ���� ũ�� transform.position���� (x)�κ��� ������ -��ŭ (y)�κ��� �� -��ŭ
    public int capacity = 3;

    float time = 0.0f;
    int count = 0;

    List<Node> spawnAreaList;
    MapArea map;

    private void Start()
    {
        map = GetComponentInParent<MapArea>();              //�̷� ���ΰ� ���θ� �����ϴ� ��Ȳ�� �̷����� �ʴ°� ���� (��ȣ����)
        spawnAreaList = map.CalcSpawnArea(this);
    }

    private void Update()
    {
        if (count < capacity) 
        {
            time += Time.deltaTime;
            if (time > interval) 
            {
                spawn();
                time = 0.0f;
            }
        }
    }

    void spawn() 
    {
        if (IsSpawnAvailable(out Vector3 spawnPosition)) 
        {
            Slime slime = Factory.Instance.GetSlime();      //��ġ������ �ʱ�ȭ �Լ� ����
            slime.Initialized(map.GridMap, spawnPosition);
            slime.OnDie += () =>
            {
                count--;
            };
            slime.transform.SetParent(transform);
            count++;
        }
    }

    bool IsSpawnAvailable(out Vector3 spawnablePosition) //out(��¿�)�� �ݵ�� ���� ��������� ��
    {
        spawnablePosition = Vector3.zero;
        List<Node> positions = new List<Node>();

        foreach (Node node in spawnAreaList) 
        {
            if (node.type == Node.NodeType.Plain) 
            {
                positions.Add(node);
            }
        }

        if (positions.Count > 0)
        {
            int index = Random.Range(0, positions.Count);
            Node target = positions[index];
            spawnablePosition = map.GridToWorld(target.X, target.Y);
            return true;
        }
        else 
        {
            spawnablePosition = Vector3.zero;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {                           //���� - �Ҽ��� ����
        Vector3 p0 = new(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Vector3 p1 = p0 + Vector3.right * size.x;
        Vector3 p2 = p0 + (Vector3)size;
        Vector3 p3 = p0 + Vector3.up * size.y; ;

        Handles.color = Color.yellow;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p0, p3, 5);
        Handles.DrawLine(p3, p2, 5);
    }
#endif
}
