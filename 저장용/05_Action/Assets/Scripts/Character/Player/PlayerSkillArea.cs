using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillArea : MonoBehaviour
{
    public float skillTick = 0.5f;      //n초당 뎀
    public float skillPower = 0.2f;     //틱당 n뎀(증폭)

    float finalPower;

    public void Activate(float power) 
    {
        finalPower = power * (1 + skillPower);

        gameObject.SetActive(true);
        //정해진 틱마다 트리거안의 모든 적에게 데미지 칼 이펙트, 지속적 mp감소, 스킬 애니메 시작
    }

    public void Deactivate() 
    {//이펙트 끈다, 스킬애니메 종료
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
