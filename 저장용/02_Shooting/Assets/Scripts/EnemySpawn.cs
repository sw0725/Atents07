using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public float timeLaps = 0.5f;
    float timer = 0.0f;
    float min = -4.0f;
    float max = 4.0f;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeLaps) 
        {
            timer = 0.0f;
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
}
