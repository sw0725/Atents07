using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : MonoBehaviour
{
    Animator animator;

    readonly int IsOpenHash = Animator.StringToHash("IsOpen");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnOpen() 
    {
    
    }

    protected virtual void OnClose()
    {
    
    }


    public void Open()
    {
        animator.SetBool(IsOpenHash, true);
        OnOpen();
    }

    public void Close()
    {
        animator.SetBool(IsOpenHash, false);
        OnClose();
    }
}