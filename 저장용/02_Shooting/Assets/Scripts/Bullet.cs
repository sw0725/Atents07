using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float moveSpeed = 7f;
    public float AttackPower = 1.0f;
    public GameObject hitEffect;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Damage(AttackPower);
        }
        Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
