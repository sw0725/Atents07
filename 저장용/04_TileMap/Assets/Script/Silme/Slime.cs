using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : RecycleObject
{
    public float phaseDuration = 0.5f;
    public float dissolveDuration = 1.0f;

    Material material;
    Action onDissolveEnd;

    const float VisialeOutLine = 0.004f;
    const float VisiablePhase = 0.1f;

    readonly int DissolveFadeId = Shader.PropertyToID("_DissolveFade");
    readonly int SplitId = Shader.PropertyToID("_Split");
    readonly int OutLineId = Shader.PropertyToID("_OutLineThickness");
    readonly int PhaseId = Shader.PropertyToID("_PhaseThickness");

    private void Awake()
    {
        Renderer slime = GetComponent<Renderer>();
        material = slime.material;

        onDissolveEnd += ReturnToPool;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        ResetShader();                      //재사용시 중간에 멈춘채로 이어시작하는것 방지용
        StartCoroutine(StartPhase());
    }

    public void Die() 
    {
        StartCoroutine(StartDissolve());
    }

    void ReturnToPool() 
    {
        gameObject.SetActive(false);
    }

    void ResetShader()
    {
        ShowOutLine(false);
        material.SetFloat(PhaseId, 0);
        material.SetFloat(SplitId, 1);
        material.SetFloat(DissolveFadeId, 1);
    }
    public void ShowOutLine(bool isShow = true)
    {
        if (isShow)
        {
            material.SetFloat(OutLineId, VisialeOutLine);
        }
        else
        {
            material.SetFloat(OutLineId, 0);
        }
    }
    IEnumerator StartPhase()
    {
        float nomalrise = 1.0f / phaseDuration;

        float time = 0.0f;

        material.SetFloat(PhaseId, VisiablePhase);
        while (time < phaseDuration)
        {
            time += Time.deltaTime;
            material.SetFloat(SplitId, 1 - (time * nomalrise));     // == time/phaseDuration **플롯나눗셈은 적으면 적을수록 좋다.

            yield return null;
        }
        material.SetFloat(PhaseId, 0); ;
        material.SetFloat(SplitId, 0); ;
    }
    IEnumerator StartDissolve()
    {
        float nomalrise = 1.0f / dissolveDuration;

        float time = 0.0f;

        while (time < dissolveDuration)
        {
            time += Time.deltaTime;
            material.SetFloat(DissolveFadeId, 1 - (time * nomalrise));

            yield return null;
        }
        material.SetFloat(DissolveFadeId, 0);

        onDissolveEnd?.Invoke();
    }
#if UNITY_EDITOR
    public void TestShader(int index) 
    {
        switch (index) 
        {
            case 1:
                ResetShader();
                break;
            case 2:
                ShowOutLine(true);
                break;
            case 3:
                ShowOutLine(false);
                break;
            case 4:
                StartCoroutine(StartPhase());
                break;
            case 5:
                StartCoroutine(StartDissolve());
                break;
        }
    }

    public void TestDie() 
    {
        Die();
    }
#endif
}
