using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : MonoBehaviour
{
    public DoorKey doorKey = null;

    Animator animator;

    readonly int IsOpenHash = Animator.StringToHash("IsOpen");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start() 
    {
        if (doorKey != null)
        {
            doorKey.onConsume += OnKeyUse;
        }
    }

    protected virtual void OnOpen() 
    {
    
    }

    protected virtual void OnClose()
    {
    
    }

    protected virtual void OnKeyUse() 
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