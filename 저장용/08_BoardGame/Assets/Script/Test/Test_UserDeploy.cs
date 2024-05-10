using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UserDeploy : TestBase
{
    public DeployToggle toggle;

    private void Start()
    {
        GameManager.Instance.GameState = GameState.ShipDeploy;
        GameManager.Instance.User.Test_BindInputSys();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(0);
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(1);
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(2);
    }
}
