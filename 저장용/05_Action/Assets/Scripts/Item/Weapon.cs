using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
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
}
