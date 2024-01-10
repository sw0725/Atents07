using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expiosion : RecycleObject
{
    Animator animator;
    float animeLenth = 0.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animeLenth = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
    // Start is called before the first frame update

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(animeLenth));
    }

}
