using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletHole : RecycleObject
{
    VisualEffect effect;
    float duration;

    readonly int OnStartEventID = Shader.PropertyToID("OnStart");
    readonly int DurationID = Shader.PropertyToID("Duration");
    readonly int DurationRangeID = Shader.PropertyToID("DurationRange");
    readonly int DebrisSpawnReflectID = Shader.PropertyToID("DebrisSpawnReflect");

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
        float durationRange = effect.GetFloat(DurationRangeID);
        duration = effect.GetFloat(DurationID) + durationRange;
    }

    public void Initialize(Vector3 pos, Vector3 normal, Vector3 reflect)
    {
        transform.position = pos;
        transform.forward = -normal;
        effect.SetVector3(DebrisSpawnReflectID, reflect);                   //파편 반사방향

        effect.SendEvent(OnStartEventID);
        StartCoroutine(LifeOver(duration));
    }
}
