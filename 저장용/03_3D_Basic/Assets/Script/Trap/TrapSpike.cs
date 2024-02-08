using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : TrapBace
{
    Animator animator;
    readonly int goHash = Animator.StringToHash("Go");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTrapActivate(GameObject gameObject)
    {
        animator.SetTrigger(goHash);
    }
}
