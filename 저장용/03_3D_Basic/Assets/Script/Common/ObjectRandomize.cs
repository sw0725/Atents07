using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomize : MonoBehaviour
{
    public bool reroll = false;

    private void Awake()
    {
        Randomize();
    }

    private void OnValidate()   //클래스의 맴버변수가 성공적으로 변경되엇을시 작동
    {
        if (reroll) 
        {
            Randomize();
            reroll = false;
        }
    }

    private void Randomize()
    {
        transform.localScale = new Vector3(1 + Random.Range(-0.15f, 0.15f), 1 + Random.Range(-0.15f, 0.15f), 1 + Random.Range(-0.15f, 0.15f));

        transform.Rotate(0, 1 + Random.Range(0, 360.0f), 0);
    }
}
