using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattler
{
    Transform transform { get; }    //이 오브제의 트랜스폼 이것은 기본지급이라 구현 안해도 됨
    float AttackPower { get; }
    float DefencePower { get; }

    void Attack(IBattler target);
    void Defence(float damage);
}
