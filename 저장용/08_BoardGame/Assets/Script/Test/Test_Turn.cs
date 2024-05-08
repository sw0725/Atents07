using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Turn : Test_PlayerBase
{
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        enemy.AutoAttack();
    }
}
