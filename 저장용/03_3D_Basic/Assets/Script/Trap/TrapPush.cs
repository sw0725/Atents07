using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPush : TrapBace
{
    public float pushPower = 5.0f;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTrapActivate(GameObject gameObject)
    {
        animator.SetTrigger("Go");
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            Vector3 dir = (transform.up + transform.forward).normalized;
            rb.AddForce(pushPower * dir, ForceMode.Impulse);
        }
    }
}
