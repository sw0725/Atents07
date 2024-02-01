using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnOff 
{ 
    idle = 0,
    on,
    off
}

public class DoorSwitch : MonoBehaviour, IInteracable
{
    public GameObject door;
    
    IInteracable interacable;
    Animator animator;
    OnOff onOff;

    bool isUsing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        onOff = OnOff.idle;
    }

    void Start() 
    {
        if (door != null) 
        {
            interacable = door.GetComponent<IInteracable>();
        }

        if (interacable == null) 
        {
            Debug.LogWarning($"{gameObject.name}에게 사용할 오브젝트 없음");
        }
    }

    public void Use()
    {
        if( interacable != null ) 
        {
            if (!isUsing) 
            {
                interacable.Use();
                StartCoroutine(ResetSwitch());
            }
        }
    }

    IEnumerator ResetSwitch() 
    {
        isUsing = true;

        switch (onOff) 
        {
            case OnOff.idle:
                animator.SetBool("IsOpen", true);
                onOff = OnOff.on;
                break;
            case OnOff.on:
                animator.SetBool("IsOpen", false);
                onOff = OnOff.off;
                break;
            case OnOff.off:
                float Timer = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                yield return new WaitForSeconds(Timer);
                onOff = OnOff.idle;
                break;
        }

        isUsing = false;
    }
}

//도어매뉴얼 리뉴얼 도어스위치 리누얼 //사용가능한것은 메뉴얼 계얼로
