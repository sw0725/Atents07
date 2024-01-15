using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AsteroidMini : TestBase
{
    public Transform Target;

    private void Start()
    {
        Target = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetAstoridMini(Target.position);
    }
}
