using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    public UserPlayer User => user;
    UserPlayer user;

    public EnemyPlayer Enemy => enemy;
    EnemyPlayer enemy;

    CinemachineImpulseSource cameraImpulseSource;

    protected override void OnpreInitialize()
    {
        base.OnpreInitialize();

        cameraImpulseSource = GetComponentInChildren<CinemachineImpulseSource>();
    }

    protected override void OnInitialize()
    {
        user = FindAnyObjectByType<UserPlayer>();
        enemy = FindAnyObjectByType<EnemyPlayer>();
    }

    public void CameraShake(float force) 
    {
        cameraImpulseSource.GenerateImpulseWithVelocity(force * Random.insideUnitCircle.normalized);
    }
}
