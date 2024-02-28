using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeSpawn : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsByType<Slime>(FindObjectsSortMode.InstanceID);      //바이타입은 오프타입과 같으나 따로 정렬방식을 지정 가능
        foreach (Slime slime in slimes) 
        {
            slime.Die();
        }
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsOfType<Slime>();
        slimes[0].Die();
    }
}
