using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expiosion : MonoBehaviour
{
    Animator animator;
    float animeLenth = 0.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animeLenth = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, animeLenth);
    }

}
