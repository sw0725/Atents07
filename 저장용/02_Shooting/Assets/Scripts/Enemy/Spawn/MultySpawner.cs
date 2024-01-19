using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultySpawner : MonoBehaviour
{
                                 //����-�����͸� ������ ������� �ִ´�(�����������)
    [System.Serializable]               //����ü�� �����Ϳ��� ������ �����ϰ� ���� -������ �Ҵ�� �޸� ��ġ�� ����(��Ÿ��) �����Ϳ��� �Ⱥ���
    public struct SpawnData 
    {
        public SpawnData(PoolObjectType type = PoolObjectType.EnemyWave, float timeLaps = 0.5f) 
        {
            this.spawnType = type;
            this.timeLaps = timeLaps;
        }

        public PoolObjectType spawnType;
        public float timeLaps;
    }

    public SpawnData[] spawnDatas;

    float min = -4.0f;
    float max = 4.0f;

    Transform astroidDestination;

    private void Awake()
    {
        astroidDestination = transform.GetChild(0);
    }

    private void Start()
    {
        foreach (var data in spawnDatas) 
        {
            StartCoroutine(SpawnCoroutine(data));
        }
    }

    private IEnumerator SpawnCoroutine(SpawnData data) 
    {
        while (true) 
        {
            yield return new WaitForSeconds(data.timeLaps);
            float hight = Random.Range(min, max);

            GameObject obj = Factory.Instance.GetObject(data.spawnType, new(transform.position.x, hight, 0.0f));

            switch (data.spawnType)
            {
                case PoolObjectType.EnemyAstroid:
                    Asteroid asteroid = obj.GetComponent<Asteroid>();
                    asteroid.SetDestination(GetDestination());
                    break;
            }
        }
    }

    Vector3 GetDestination()
    {
        Vector3 pos = astroidDestination.position;
        pos.y += Random.Range(min, max);  // ���� ��ġ���� ���̸� (-4 ~ +4) ����

        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 p0 = transform.position + Vector3.up * max;
        Vector3 p1 = transform.position + Vector3.up * min;
        Gizmos.DrawLine(p0, p1);

        if (astroidDestination == null)
        {
            astroidDestination = transform.GetChild(0);
        }
        Gizmos.color = Color.yellow;
        Vector3 p2 = astroidDestination.position + Vector3.up * max;
        Vector3 p3 = astroidDestination.position + Vector3.up * min;
        Gizmos.DrawLine(p2, p3);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 p0 = transform.position + Vector3.up * max - Vector3.right * 0.5f;
        Vector3 p1 = transform.position + Vector3.up * min - Vector3.right * 0.5f;
        Vector3 p2 = transform.position + Vector3.up * max + Vector3.right * 0.5f;
        Vector3 p3 = transform.position + Vector3.up * min + Vector3.right * 0.5f;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p0, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p1, p3);

        Gizmos.color = Color.red;
        Vector3 p4 = astroidDestination.position + Vector3.up * max - Vector3.right * 0.5f;
        Vector3 p5 = astroidDestination.position + Vector3.up * min - Vector3.right * 0.5f;
        Vector3 p6 = astroidDestination.position + Vector3.up * max + Vector3.right * 0.5f;
        Vector3 p7 = astroidDestination.position + Vector3.up * min + Vector3.right * 0.5f;
        Gizmos.DrawLine(p4, p5);
        Gizmos.DrawLine(p4, p6);
        Gizmos.DrawLine(p6, p7);
        Gizmos.DrawLine(p5, p7);
    }
}
