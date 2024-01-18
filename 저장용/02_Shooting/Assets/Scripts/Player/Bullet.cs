using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    float moveSpeed = 7f;
    public float lifeTime = 10.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Factory.Instance.GetHitEffect(transform.position);
        gameObject.SetActive(false);
    }
}
