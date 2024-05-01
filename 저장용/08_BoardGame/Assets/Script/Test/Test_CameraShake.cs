using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_CameraShake : TestBase
{
    public CinemachineImpulseSource source;
    [Range(0,5)]
    public float force =1;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        source.GenerateImpulse();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {                                           //인사이드라 값이 1이 아닐수도 있어서 1로 정규화함
        source.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized);                 //source.GenerateImpulseAtPositionWithVelocity(); 어디위치에 어느방향으로 충격 생성
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.CameraShake(force);
    }
}
