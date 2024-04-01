using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    Transform fillPivot;

    private void Awake()
    {
        fillPivot = transform.GetChild(1);

        IHealth target = transform.GetComponentInParent<IHealth>();
        target.onHealthCange += Refresh;
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;        //카메라 회전과 일치화 => 카메라의 정면으로 놓이도록
    }

    void Refresh(float range) 
    {
        fillPivot.localScale = new(range, 1, 1);
    }
}
