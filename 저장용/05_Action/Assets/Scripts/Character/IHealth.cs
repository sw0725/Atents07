using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float HP { get; set; }      //프로퍼티도 함수다

    float MaxHP { get; }

    Action<float> onHealthCange { get; set; }   //앳션은 변수라 프로퍼티로 만들고 변수처럼사용  //float은 비율

    bool IsAlive { get; }

    void Die();

    Action onDie { get; set; }

    void HealthRegernerate(float totalRegen, float duration);       //지속회복 초당 total/duration 만큼 회복 회복 총량, 총 회복 시간
    void HealthRegernerateByTick(float tickRegen, float tickInterval, uint totalTickCount); //1틱당 회복량, 틱 간격, 총 틱 수
}
