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

    public TurnManager TurnManager => turnManager;
    TurnManager turnManager;

    public bool IsTestMode = false;

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

        turnManager = GetComponent<TurnManager>();
        turnManager.OnInitialize(user, enemy);
    }

    public void CameraShake(float force) 
    {
        cameraImpulseSource.GenerateImpulseWithVelocity(force * Random.insideUnitCircle.normalized);
    }
}
