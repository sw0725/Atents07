using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemPickUp : TestBase
{
    public ItemCode code = ItemCode.Ruby;
    public uint count = 5;
    public Transform target;

#if UNITY_EDITOR

    private void Start()
    {

    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItems(code, count, target.position, true);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {

    }
#endif
}
