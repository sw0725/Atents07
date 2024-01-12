using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawn : EnemySpawn
{
    Transform destinationArea;

    private void Awake()
    {
        destinationArea = transform.GetChild(0);    
    }

    protected override void Spawn()
    {
        Asteroid asteroid = Factory.Instance.GetAstorid(EPotition());
        asteroid.SetDestination(GetDestination());
    }

    Vector3 GetDestination()
    {
        Vector3 pos = destinationArea.position;
        pos.y += Random.Range(min, max);  // 현재 위치에서 높이만 (-4 ~ +4) 변경

        return pos;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (destinationArea == null) 
        {
            destinationArea = transform.GetChild(0);
        }
        Gizmos.color = Color.yellow;
        Vector3 p0 = destinationArea.position + Vector3.up * max;
        Vector3 p1 = destinationArea.position + Vector3.up * min;
        Gizmos.DrawLine(p0, p1);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Vector3 p0 = destinationArea.position + Vector3.up * max - Vector3.right * 0.5f;
        Vector3 p1 = destinationArea.position + Vector3.up * min - Vector3.right * 0.5f;
        Vector3 p3 = destinationArea.position + Vector3.up * max + Vector3.right * 0.5f;
        Vector3 p4 = destinationArea.position + Vector3.up * min + Vector3.right * 0.5f;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p0, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p1, p4);
    }

}
