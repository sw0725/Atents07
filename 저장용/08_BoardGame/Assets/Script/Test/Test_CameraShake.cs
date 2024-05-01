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
    {                                           //�λ��̵�� ���� 1�� �ƴҼ��� �־ 1�� ����ȭ��
        source.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized);                 //source.GenerateImpulseAtPositionWithVelocity(); �����ġ�� ����������� ��� ����
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.CameraShake(force);
    }
}
