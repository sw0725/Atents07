using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    ParticleSystem particle;
    CapsuleCollider bladeCollider;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        bladeCollider = GetComponent<CapsuleCollider>();
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
}
