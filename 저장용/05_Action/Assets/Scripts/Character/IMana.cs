using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana
{
    float MP { get; set; }      //������Ƽ�� �Լ���

    float MaxMP { get; }

    Action<float> onManaCange { get; set; }   //�ܼ��� ������ ������Ƽ�� ����� ����ó�����  //float�� ����

    void ManaRegernerate(float totalRegen, float duration);       //����ȸ�� �ʴ� total/duration ��ŭ ȸ�� ȸ�� �ѷ�, �� ȸ�� �ð�
    void ManaRegernerateByTick(float tickRegen, float tickInterval, uint totalTickCount); //1ƽ�� ȸ����, ƽ ����, �� ƽ ��
}
