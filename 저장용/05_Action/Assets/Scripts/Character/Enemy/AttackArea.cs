using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackArea : MonoBehaviour         //공격을 두번 맞는 버그 수정
{
    public SphereCollider attackArea;           //프리펩창에서 기즈모를 그릴수 있도록 퍼블릭 함
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
            Handles.DrawWireDisc(transform.position, transform.up, attackArea.radius, 5);   //공격범위
        }
    }
#endif
}
