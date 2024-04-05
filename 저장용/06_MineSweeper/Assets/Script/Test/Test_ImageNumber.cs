using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ImageNumber : TestBase
{
    [Range(-99, 999)]
    public int testNumber = 0;
    public ImageNumber imageNumber;

    public int flagCount = 5;
    public GameManager.GameState gameState;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
       imageNumber.Number = testNumber;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        imageNumber.Number = Random.Range(-99, 999);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.Test_SetFlagCount(flagCount);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        GameManager.Instance.Test_StateChange(gameState);
    }
}
