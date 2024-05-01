using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    CinemachineImpulseSource cameraImpulseSource;

    protected override void OnpreInitialize()
    {
        base.OnpreInitialize();

        cameraImpulseSource = GetComponentInChildren<CinemachineImpulseSource>();
    }

    public void CameraShake(float force) 
    {
        cameraImpulseSource.GenerateImpulseWithVelocity(force * Random.insideUnitCircle.normalized);
    }
}
