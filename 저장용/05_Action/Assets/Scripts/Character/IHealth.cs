using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float HP { get; set; }      //������Ƽ�� �Լ���

    float MaxHP { get; }

    Action<float> onHealthCange { get; set; }   //�ܼ��� ������ ������Ƽ�� ����� ����ó�����  //float�� ����

    bool IsAlive { get; }

    void Die();

    Action onDie { get; set; }

    void HealthRegernerate(float totalRegen, float duration);       //����ȸ�� �ʴ� total/duration ��ŭ ȸ�� ȸ�� �ѷ�, �� ȸ�� �ð�
    void HealthRegernerateByTick(float tickRegen, float tickInterval, uint totalTickCount); //1ƽ�� ȸ����, ƽ ����, �� ƽ ��
}
