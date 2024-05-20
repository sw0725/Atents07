using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test02_Crosshair : TestBase
{
    public Crosshair crosshair;
    public float amount = 30.0f;

    protected override void OnLClick(InputAction.CallbackContext context)
    {
        crosshair.Expend(amount);
    }
}
