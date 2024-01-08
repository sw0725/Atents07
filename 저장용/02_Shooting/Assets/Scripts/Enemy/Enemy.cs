using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Effect;

    public float moveSpeed = 1.0f;
    public float amplitude = 3.0f;
    public float frequency = 2.0f;
    public int hp = 3;
    public int score = 10;


    float spawnY = 0.0f;
    float totalTime = 0.0f;
    private int HP 
    {
        get => hp;
        set 
        {
            hp = value;
            if (hp < 0.1)
            {
                hp = 0;
                OnDie();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnY = transform.position.y;
        totalTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime * frequency;

        transform.position = new Vector3(transform.position.x-Time.deltaTime*moveSpeed, spawnY+Mathf.Sin(totalTime)*amplitude, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) 
        {
            HP--;
        }
    }

    private void OnDie() 
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Player player = FindAnyObjectByType<Player>();
        player.AddScore(score);
        Destroy(this.gameObject);
    }

}
