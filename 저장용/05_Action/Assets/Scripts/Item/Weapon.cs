using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    ParticleSystem particle;
    CapsuleCollider bladeCollider;
    Player player;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        bladeCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    public void EffectEnable(bool enable) 
    {
        if (enable)
        {
            particle.Play();
        }
        else 
        {
            particle.Stop();
        }
    }

    public void bladeColliderEnable(bool isEnable) 
    {
        bladeCollider.enabled = isEnable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            IBattler target = other.GetComponent<IBattler>();
            if (target != null) 
            {
                player.Attack(target);

                Vector3 impactPoint = transform.position + transform.up * 0.8f;     //칼 피벗에서 y축으로 0.8위쪽을 기준점으로 잡음
                Vector3 effectPoint = other.ClosestPoint(impactPoint);              //콜라이더와 부딫혔을때 기준점과 가장 가까운곳을 선정
                Factory.Instance.GetPlayerHitEffect(effectPoint);                         //해당 자리에 이펙트 소환
            }
        }
    }
}
