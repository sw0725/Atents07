using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotater : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    public float moveSpeed = 2.0f;
    public float minHight = 0.5f;
    public float maxHight = 1.5f;

    private void Start()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);
    }

    private void Update()
    {
        transform.Rotate(0, transform.rotation.y + Time.deltaTime * rotateSpeed, 0);
        //transform.position.y = minHight + Mathf.Sin();
    }
}
