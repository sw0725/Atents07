using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : EnemyBase
{
    [Header("Boss Data")]

    public PoolObjectType bullet = PoolObjectType.EnemyBossBullet;
    public PoolObjectType misslie = PoolObjectType.EnemyBossMissle;

    public float bulletInterval = 1.0f;
    public int barrageCount = 3;

    public Vector2 areamin = new Vector2(2,-3);
    public Vector2 areamax = new Vector2(7, 3); //보스 활동영역(사각형) 을 두 점으로(대각선 상의) 표현

    Transform fire1;
    Transform fire2;
    Transform fire3;
    Vector3 moveDirection = Vector3.left;

    private void Awake()
    {
        Transform firePlace = transform.GetChild(1);
        fire1 = firePlace.GetChild(0);
        fire2 = firePlace.GetChild(1);
        fire3 = firePlace.GetChild(2);
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * moveDirection);
    }

    IEnumerator MoveProsess() 
    {
        moveDirection = Vector3.left;
        float middel = (areamax.x - areamin.x) * 0.5f + areamin.x;

        while(transform.position.x > middel)
        {
            yield return null;  //다음프레임까지 기다리기
        }

        StartCoroutine(FireBullet());
        ChangeDirection();

        while (true) 
        {
            if (transform.position.y > areamax.y || transform.position.y < areamin.y)
            {
                ChangeDirection();
                StartCoroutine(FireMissle());
            }
            yield return null;
        }

    }

    void ChangeDirection() 
    {
        Vector3 target = new Vector3();
        target.x = Random.Range(areamin.x, areamax.x);
        target.y = (transform.position.y > 0) ? areamin.y : areamax.y;

        moveDirection = (target - transform.position).normalized;
    }

    public void OnSpawn() 
    {
        StopAllCoroutines();
        StartCoroutine(MoveProsess());
    }

    IEnumerator FireBullet() 
    {
        while(true) 
        {
            Factory.Instance.GetObject(PoolObjectType.EnemyBossBullet, fire1.position);
            Factory.Instance.GetObject(PoolObjectType.EnemyBossBullet, fire2.position);

            yield return new WaitForSeconds(bulletInterval);
        }
    }

    IEnumerator FireMissle() 
    {
        for(int i = 0; i < barrageCount; i++) 
        {
            Factory.Instance.GetObject(PoolObjectType.EnemyBossMissle, fire3.position);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 P0 = new Vector3(areamin.x, areamin.y);
        Vector3 P1 = new Vector3(areamax.x, areamin.y);
        Vector3 P2 = new Vector3(areamin.x, areamax.y);
        Vector3 P3 = new Vector3(areamax.x, areamax.y);

        Gizmos.DrawLine(P0, P1);
        Gizmos.DrawLine(P0, P2);
        Gizmos.DrawLine(P2, P3);
        Gizmos.DrawLine(P1, P3);
    }

}
