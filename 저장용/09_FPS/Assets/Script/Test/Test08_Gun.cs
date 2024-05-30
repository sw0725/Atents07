using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test08_Gun : TestBase
{
    public Revolver gun;
    public ShotGun shotGun;
    public AssaultRifle assaultRifle;

    private void Start()
    {
        gun.Equip();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        gun.TestFire();
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        gun.Reload();
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        shotGun.TestFire();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        assaultRifle.TestFire(!context.canceled);
    }
}
