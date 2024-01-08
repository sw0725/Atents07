using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public float timeLaps = 2.0f;
    float min = -4.0f;
    float max = 4.0f;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;

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

    void Spawn() 
    {
        GameObject gameObject = Instantiate(enemy, EPotition(), Quaternion.identity);
        gameObject.transform.SetParent(transform);
        gameObject.name = $"Enemy_{counter}";
        counter++;
    }

    Vector3 EPotition() 
    {
        Vector3 pos = transform.position;
        pos.y += Random.Range(min, max);
        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;                             
        Vector3 p0 = transform.position + Vector3.up * max;    
        Vector3 p1 = transform.position + Vector3.up * min;    
        Gizmos.DrawLine(p0, p1);                                
    }

    private void OnDrawGizmosSelected()
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
