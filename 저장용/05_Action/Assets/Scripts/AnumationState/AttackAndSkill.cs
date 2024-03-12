using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndSkill : StateMachineBehaviour
{
    Player player;

    readonly int Attack_Hash = Animator.StringToHash("Attack");

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (player == null) 
        {
            player = GameManager.Instance.Player;
        }
        player.ShowWeaponEffect(true);
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        player.ShowWeaponEffect(false);
        animator.ResetTrigger(Attack_Hash);
    }
}
