using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Enemys : TestBase
{
    Transform spawnPoint;

    private void Start()
    {
        spawnPoint = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetBouns(spawnPoint.position);
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetCurve(spawnPoint.position);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetBossBullet(spawnPoint.position);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Factory.Instance.GetBossMissle(spawnPoint.position);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        //boss
    }
}
