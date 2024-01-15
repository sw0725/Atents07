using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Asteroid : EnemyBase
{
    [Header("Asteroid data")]

    Vector3 direction = Vector3.zero;
    int originalScore;

    public float rotateSpeed = 360.0f;
    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;

    public float minSpeed = 2.0f;
    public float maxSpeed = 4.0f;
    
    public float minLifeTime = 4.0f;
    public float maxLifeTime = 7.0f;
    
    public int minMiniCount = 3;
    public int maxMiniCount = 8;

    [Range(0, 1)]
    public float criticalRate = 0.05f;
    public int criticalMiniCount = 20;

    private void Awake()
    {
        originalScore = score;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        score = originalScore;

        StartCoroutine(SelfCrush());
    }

    public void SetDestination(Vector3 destination) 
    {
        direction = (destination - transform.position).normalized;
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
        transform.Translate(Time.deltaTime * moveSpeed * direction, Space.World);
    }

    protected override void OnDie()
    {
        int count = criticalMiniCount;

        if (Random.value > criticalRate) //value=0-1사이
        {
            count = Random.Range(minMiniCount, maxMiniCount);
        }
        float angle = 360 / count;
        float startAngle = Random.Range(0, 360.0f);
        for (int i = 0; i < count; i++) 
        {
            Factory.Instance.GetAstoridMini(transform.position, startAngle+angle*i);
        }
        base.OnDie();//서-순
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

    IEnumerator SelfCrush() 
    {
        float lifTime = Random.Range(minLifeTime, maxLifeTime);
        yield return new WaitForSeconds(lifTime);
        score = 0;
        OnDie();
    }
}
