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

                Vector3 impactPoint = transform.position + transform.up * 0.8f;     //Į �ǹ����� y������ 0.8������ ���������� ����
                Vector3 effectPoint = other.ClosestPoint(impactPoint);              //�ݶ��̴��� �΋H������ �������� ���� �������� ����
                Factory.Instance.GetPlayerHitEffect(effectPoint);                         //�ش� �ڸ��� ����Ʈ ��ȯ
            }
        }
    }
}
