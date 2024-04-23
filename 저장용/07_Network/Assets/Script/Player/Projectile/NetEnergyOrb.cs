using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class NetEnergyOrb : MonoBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 20.0f;
    public float expolsionRadius = 5.0f;
    public Action OnEffectFinish;

    Rigidbody rb;
    VisualEffect effect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        effect = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        transform.Rotate(-30.0f, 0, 0);
        rb.velocity = speed * transform.forward;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] result = Physics.OverlapSphere(transform.position, expolsionRadius, LayerMask.GetMask("Player"));

        if (result.Length > 0) 
        {
            foreach(Collider col in result) 
            {
                Debug.Log(col.gameObject.name);
            }
        }

        StartCoroutine(EffectFinishProcess());
    }

    IEnumerator EffectFinishProcess() 
    {
        int id = Shader.PropertyToID("Size");                   //이펙트의 프로퍼티 가져오기
        float timer = 0.0f;
        while (timer < 0.5f) 
        {
            timer += Time.deltaTime;
            float radi = 1.0f + expolsionRadius * timer/0.5f;
            effect.SetFloat(id, radi);                          //프로퍼티 값 수정

            yield return null;
        }
        timer = 0.0f;
        while (timer < 1.0f)
        {
            timer += Time.deltaTime;
            float radi = expolsionRadius - expolsionRadius * timer;
            effect.SetFloat(id, radi);                          //프로퍼티 값 수정

            yield return null;
        }
        OnEffectFinish?.Invoke();
        while (effect.aliveParticleCount >= 0)      //이펙트의 파티클 갯수가 0이 되면 
        {
            if (effect.aliveParticleCount == 0)
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
