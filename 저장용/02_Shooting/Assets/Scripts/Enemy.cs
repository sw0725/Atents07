using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public float amplitude = 3.0f;
    
    public float frequency = 2.0f;

    float spawnY = 0.0f;
    
    float totalTime = 0.0f;

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
}
