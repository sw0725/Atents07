using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour, IInteracable
{   
    public DoorManual door;
    public float coolTime = 0.5f;
    float coolTimer = 0;
    public bool CanUse => coolTimer < 0.0f;

    Animator animator;
    State onOff = State.off;

    readonly int SwitchOnHash = Animator.StringToHash("On");


    enum State
    {
        off =0,
        on
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start() 
    {
        if (door == null) 
        {
            Debug.LogWarning($"{gameObject.name}에게 DoorManual 없음");
        }
    }
    private void Update()
    {
        coolTimer -= Time.deltaTime;
    }

    public void Use()
    {
        if(door != null && CanUse) 
        {
            switch (onOff) 
            {
                case State.off:
                    door.Open();
                    animator.SetBool(SwitchOnHash, true);
                    onOff = State.on;
                    break;
                case State.on:
                    door.Close();
                    animator.SetBool(SwitchOnHash, false);
                    onOff = State.off;
                    break;   
            }
        }
    }
}
