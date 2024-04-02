using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattler
{
    Transform transform { get; }    //�� �������� Ʈ������ �̰��� �⺻�����̶� ���� ���ص� ��
    float AttackPower { get; }
    float DefencePower { get; }

    Action<int> onHit { get; set; }

    void Attack(IBattler target);
    void Defence(float damage);
}
