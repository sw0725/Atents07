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
        transform.rotation = Camera.main.transform.rotation;        //ī�޶� ȸ���� ��ġȭ => ī�޶��� �������� ���̵���
    }

    void Refresh(float range) 
    {
        fillPivot.localScale = new(range, 1, 1);
    }
}
