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
        postProcessVolume.profile.TryGet<Vignette>(out vignette);       //볼륨에서 비네트 가져오기 시도(비네트는 볼륨의 프로파일임)
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
