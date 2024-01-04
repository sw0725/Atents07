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
    public float maxHp = 3.0f;


    float spawnY = 0.0f;
    float totalTime = 0.0f;
    float hp = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnY = transform.position.y;
        totalTime = 0.0f;

        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime * frequency;

        transform.position = new Vector3(transform.position.x-Time.deltaTime*moveSpeed, spawnY+Mathf.Sin(totalTime)*amplitude, 0.0f);
    }

    public void Damage(float attck) 
    {
        hp -= attck;
        hp = math.clamp(hp, 0.0f, maxHp);
        if (hp < 0.1f) 
        {
            Instantiate(Effect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
