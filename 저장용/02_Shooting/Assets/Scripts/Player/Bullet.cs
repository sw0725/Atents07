using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float moveSpeed = 7f;
    public int AttackPower = 1;
    public GameObject hitEffect;
    public float lifeTime = 10.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(hitEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
