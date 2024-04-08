using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TestBoard : TestBase
{
    public int width = 8;
    public int height = 8;
    public int mine = 3;

    Board board;

    private void Start()
    {
        board = FindAnyObjectByType<Board>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        board.Initialize(width, height, mine);
        board.Test_OpenAllCover();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        board.Test_BoardReset();
        board.Test_OpenAllCover();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        base.OnTest3(context);
    }
}
