using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : RecycleObject
{
    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();            //파티클이 지속되는 시간
        StartCoroutine(LifeOver(particle.main.duration));
    }
}
