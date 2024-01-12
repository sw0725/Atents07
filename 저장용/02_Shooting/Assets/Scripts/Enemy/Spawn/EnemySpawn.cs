using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float timeLaps = 2.0f;
    protected float min = -4.0f;
    protected float max = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeLaps);
            Spawn();
        }
    }

    protected virtual void Spawn() 
    {
        Wave enemy = Factory.Instance.GetEnemyWave(EPotition());
    }

    protected Vector3 EPotition() 
    {
        Vector3 pos = transform.position;
        pos.y += Random.Range(min, max);
        return pos;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;                             
        Vector3 p0 = transform.position + Vector3.up * max;    
        Vector3 p1 = transform.position + Vector3.up * min;    
        Gizmos.DrawLine(p0, p1);                                
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 p0 = transform.position + Vector3.up * max - Vector3.right * 0.5f;
        Vector3 p1 = transform.position + Vector3.up * min - Vector3.right * 0.5f;
        Vector3 p3 = transform.position + Vector3.up * max + Vector3.right * 0.5f;
        Vector3 p4 = transform.position + Vector3.up * min + Vector3.right * 0.5f;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p0, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p1, p4);
    }
}
