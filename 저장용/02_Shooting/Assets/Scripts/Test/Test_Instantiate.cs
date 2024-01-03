using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Instantiate : TestBase
{
    public GameObject prefab;
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(prefab, this.transform);
    }
}
