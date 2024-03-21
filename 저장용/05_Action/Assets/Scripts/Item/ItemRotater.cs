using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotater : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    public float moveSpeed = 2.0f;
    public float minHight = 0.5f;
    public float maxHight = 1.5f;

    float time = 0.0f;

    private void Start()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);
        transform.position = transform.parent.position + Vector3.up * maxHight;
    }

    private void Update()
    {
        time += Time.deltaTime * moveSpeed;

        Vector3 pos;
        pos.x = transform.parent.position.x;
        pos.y = minHight + ((Mathf.Cos(time) + 1) * 0.5f) * (maxHight - minHight);
        pos.z = transform.parent.position.z;             //Cos() + 1 = 0 ~ 2  //  (Cos() + 1) * 0.5 = 0 ~ 1  //  ((Cos() + 1) * 0.5) * (max - min) = 0 ~ (max - min)
        transform.position = pos;                          //  min + ((Cos() + 1) * 0.5) = min ~ (min + 1)  //  min + ((Cos() + 1) * 0.5) * (max - min) = min ~ max

        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
    }
}
