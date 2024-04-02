using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackArea : MonoBehaviour         //������ �ι� �´� ���� ����
{
    public SphereCollider attackArea;           //������â���� ����� �׸��� �ֵ��� �ۺ� ��
    public Action<IBattler> onPlayerIn;
    public Action<IBattler> onPlayerOut;

    private void Awake()
    {
        attackArea = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattler target = other.GetComponent<IBattler>();
            onPlayerIn?.Invoke(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattler target = other.GetComponent<IBattler>();
            onPlayerOut?.Invoke(target);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackArea != null)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, attackArea.radius, 5);   //���ݹ���
        }
    }
#endif
}
