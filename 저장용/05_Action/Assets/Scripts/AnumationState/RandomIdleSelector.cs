using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class RandomIdleSelector : StateMachineBehaviour
{
    readonly int IdleSelect_Hash = Animator.StringToHash("IdleSelect");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(IdleSelect_Hash, RandomSelect());
    }

    int RandomSelect() 
    {
        int select = 0;
        float num = Random.value;

        if (num < 0.05f)        //5��
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
        else
        {
            select = 0;
        }
        return select;
    }
}
