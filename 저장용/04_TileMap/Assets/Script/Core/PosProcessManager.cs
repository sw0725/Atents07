using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PosProcessManager : MonoBehaviour
{
    Volume postProcessVolume;
    Vignette vignette;

    public AnimationCurve curve;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);       //�������� ���Ʈ �������� �õ�(���Ʈ�� ������ ����������)
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onLifeTimeChange += LifeTimeChange;
    }

    private void LifeTimeChange(float ratio)
    {
        vignette.intensity.value = curve.Evaluate(ratio);
    }
}
