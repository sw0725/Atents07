using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PowerUp : TestBase
{
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.Test_PowerUP();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Test_PowerDown();
    }
}
