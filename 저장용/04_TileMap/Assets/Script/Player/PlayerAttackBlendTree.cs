using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBlendTree : StateMachineBehaviour              //Start����
{
    //���� ���Խ� ȣ��(Ʈ������ �������� ����Ѵ�)
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //�ִϸ��̼� ������ ȣ��
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    Player player;

    private void OnEnable()
    {
        player = GameManager.Instance.Player;
    }

    //���� ����� ȣ��(Ʈ������ �������� ����Ѵ�)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.RestorSpeed();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // �ι��� Ű�׸�ƽ(������ ���¿� ���߾� �ִϸ� �����ϴ� �Լ�)ȣ���� ȣ��
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
