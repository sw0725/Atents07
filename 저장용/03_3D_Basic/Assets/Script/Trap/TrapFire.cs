using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapFire : TrapBace
{
    public float duration = 5.0f;

    ParticleSystem ps;

    private void Awake()
    {
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject gameObject)
    {
        ps.Play();

        IAive aive = gameObject.GetComponent<IAive>();
        if (aive != null ) 
        {
            aive.Die();
        }
        StopAllCoroutines();
        StartCoroutine(StopEffect());
    }

    IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(duration);
        ps.Stop();
    }

}
