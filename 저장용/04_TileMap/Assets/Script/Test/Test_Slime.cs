using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime : TestBase
{
    public Renderer[] slime;

    public float Speed = 0.5f;
    public bool LineThicknessChange = false;
    public bool SplitChange = false;
    public bool phaseThicknessChange = false;
    public bool DissolvFadeChange = false;

    [Range(0.0f, 1.0f)]
    public float split = 0.0f;
    [Range(0.1f, 2.0f)]
    public float phaseThick = 0.0f;
    [Range(0.001f, 0.01f)]
    public float LineThick = 0.0f;
    [Range(0.0f, 1.0f)]
    public float DissolveFade = 0.0f;

    float time = 0.0f;

    readonly int SplitId = Shader.PropertyToID("_Split");
    readonly int ReverseSplitId = Shader.PropertyToID("_Split");
    readonly int OutLineThickness = Shader.PropertyToID("_Thickness");      //메터리얼 속성에 접근하려거든 속성이름 앞에 "_"필수
    readonly int SplitThickness = Shader.PropertyToID("_PhaseThickness");
    readonly int ReverseThickness = Shader.PropertyToID("_PhaseThickness");
    readonly int InnerLineThickness = Shader.PropertyToID("_InnerThickness");
    readonly int DissolveFadeId = Shader.PropertyToID("_DissolveFade");

    Material[] materials;

    private void Start()
    {
        materials = new Material[slime.Length];
        for (int i = 0; i < materials.Length; i++) 
        {
            materials[i] = slime[i].material;
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        float num = (Mathf.Cos(time * Speed) + 1.0f) * 0.5f;

        if(LineThicknessChange)
        {
            float min = 0.001f;
            float max = 0.01f;
            float resulte = min + (max - min) * num;

            materials[0].SetFloat(OutLineThickness, resulte);
            materials[3].SetFloat(InnerLineThickness, resulte);
            LineThick = resulte;
        }
        if (SplitChange) 
        {
            materials[1].SetFloat(SplitId, num);
            materials[2].SetFloat(ReverseSplitId, num);
            split = num;
        }
        if (phaseThicknessChange) 
        {
            float min = 0.1f;
            float max = 0.2f;
            float resulte = min + (max - min) * num;

            materials[1].SetFloat(SplitThickness, resulte);
            materials[2].SetFloat(ReverseThickness, resulte);
            phaseThick = resulte;
        }
        if (DissolvFadeChange)
        {
            materials[4].SetFloat(DissolveFadeId, num);
            DissolveFade = num;
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        LineThicknessChange = !LineThicknessChange;
    }//아웃라인 두깨
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        SplitChange = !SplitChange;
    }//리버스도 적용
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        phaseThicknessChange = !phaseThicknessChange;
    }//페이즈 리버스두깨
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        DissolvFadeChange = !DissolvFadeChange;
    }
}
