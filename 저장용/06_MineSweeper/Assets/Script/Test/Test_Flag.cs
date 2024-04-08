using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Flag : TestBase
{
    Board board;

    private void Start()
    {
        board = GameManager.Instance.Board;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        board.Test_BoardReset();
    }
}
///열린셀 누를시 주위 모든셀이 눌림, 단 깃발은 제외 땟을때 깃발과 지뢰가 갯수가 안맞으면 잠김 맞으면 연다.