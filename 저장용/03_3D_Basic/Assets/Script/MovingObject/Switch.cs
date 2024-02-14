using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteracable
{
    public float coolTime = 0.5f;
    float coolTimer = 0;
    public bool CanUse => coolTimer < 0.0f;

    Animator animator;
    IInteracable target;
    State onOff = State.on;

    readonly int IsUseHash = Animator.StringToHash("IsUse");


    enum State
    {
        off = 0,
        on
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        target = transform.parent.GetComponent<IInteracable>();

        if (target == null)
        {
            Debug.LogWarning($"{gameObject.name}에게 사용할 오브젝트 없음");
        }
    }
    private void Update()
    {
        coolTimer -= Time.deltaTime;
    }

    public void Use()
    {
        if (target != null && CanUse)
        {
            switch (onOff)
            {
                case State.off:
                    animator.SetBool(IsUseHash, true);
                    onOff = State.on;
                    break;
                case State.on:
                    animator.SetBool(IsUseHash, false);
                    onOff = State.off;
                    break;
            }

            target.Use();
        }
    }
}
