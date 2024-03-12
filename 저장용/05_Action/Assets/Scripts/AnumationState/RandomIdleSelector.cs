using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class RandomIdleSelector : StateMachineBehaviour
{
    public int test = -1;
    readonly int IdleSelect_Hash = Animator.StringToHash("IdleSelect");

    int prevSelect = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(IdleSelect_Hash, RandomSelect());
    }

    int RandomSelect() 
    {
        int select = 0;
        
        if (prevSelect == 0) 
        {
            float num = Random.value;

            if (num < 0.05f)        //5ÆÛ
            {
                select = 4;
            }
            else if (num < 0.10f)
            {
                select = 3;
            }
            else if (num < 0.15f)
            {
                select = 2;
            }
            else if (num < 0.20f)
            {
                select = 1;
            }
        }
        if (test != -1) 
        {
            select = test;
        }
        
        prevSelect = select;
        return select;
    }
}
