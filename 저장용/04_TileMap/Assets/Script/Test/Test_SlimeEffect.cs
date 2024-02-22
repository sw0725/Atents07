using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeEffect : TestBase
{
    public Renderer Slime;

    const float VisialeOutLine = 0.004f;
    const float VisiablePhase = 0.1f;

    readonly int DissolveFadeId = Shader.PropertyToID("_DissolveFade");
    readonly int SplitId = Shader.PropertyToID("_Split");
    readonly int OutLineId = Shader.PropertyToID("_OutLineThickness");
    readonly int PhaseId = Shader.PropertyToID("_PhaseThickness");

    Material material;

    private void Start()
    {
        material = Slime.material;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        ResetShader();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        ShowOutLine(true);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        ShowOutLine(false);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        StartCoroutine(StartPhase());
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        StartCoroutine(StartDissolve());
    }

    void ResetShader() 
    {
        ShowOutLine(false);
        material.SetFloat(PhaseId, 0);
        material.SetFloat(SplitId, 1);
        material.SetFloat(DissolveFadeId, 1);
    }
    void ShowOutLine(bool isShow) 
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
        float duration = 0.5f;
        float nomalrise = 1.0f / duration;

        float time = 0.0f;

        material.SetFloat(PhaseId, VisiablePhase);
        while (time < duration) 
        {
            time += Time.deltaTime;
            material.SetFloat(SplitId, 1-(time*nomalrise));     // == time/duration **ÇÃ·Ô³ª´°¼ÀÀº ÀûÀ¸¸é ÀûÀ»¼ö·Ï ÁÁ´Ù.

            yield return null;
        }
        material.SetFloat(PhaseId, 0);;
        material.SetFloat(SplitId, 0);;
    }
    IEnumerator StartDissolve() 
    {
        float duration = 0.5f;
        float nomalrise = 1.0f / duration;

        float time = 0.0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            material.SetFloat(DissolveFadeId, 1 - (time * nomalrise));

            yield return null;
        }
        material.SetFloat(DissolveFadeId, 0); ;
    }
}
