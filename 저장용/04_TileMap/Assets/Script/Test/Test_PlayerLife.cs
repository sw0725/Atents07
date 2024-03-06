using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerLife : TestBase
{
    public ImageNumber ImageNumber;
    public int number;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        ImageNumber.Number = number;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsByType<Slime>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Slime slime in slimes) 
        {
            slime.Die();
        }
    }
#if UNITY_EDITOR
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.TestDie();
    }
#endif
}
