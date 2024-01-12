using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundPlanet : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public float minEnd = 30.0f;
    public float maxEnd = 60.0f;
    public float minY = -8.0f;
    public float maxY = -5.0f;

    float baceLineX;

    private void Start()
    {
        baceLineX = transform.position.x;
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * MoveSpeed * -transform.right);

        if (transform.position.x < baceLineX) 
        {
            transform.position = new Vector3(Random.Range(minEnd, maxEnd), Random.Range(minY, maxY), 0);
        }
    }
}
