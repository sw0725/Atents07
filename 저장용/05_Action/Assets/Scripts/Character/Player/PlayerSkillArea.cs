using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillArea : MonoBehaviour
{
    public float skillTick = 0.5f;      //n�ʴ� ��
    public float skillPower = 0.2f;     //ƽ�� n��(����)

    float finalPower;

    public void Activate(float power) 
    {
        finalPower = power * (1 + skillPower);

        gameObject.SetActive(true);
        //������ ƽ���� Ʈ���ž��� ��� ������ ������ Į ����Ʈ, ������ mp����, ��ų �ִϸ� ����
    }

    public void Deactivate() 
    {//����Ʈ ����, ��ų�ִϸ� ����
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
