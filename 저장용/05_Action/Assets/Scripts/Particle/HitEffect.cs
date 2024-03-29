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
        base.OnEnable();            //��ƼŬ�� ���ӵǴ� �ð�
        StartCoroutine(LifeOver(particle.main.duration));
    }
}
