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
///������ ������ ���� ��缿�� ����, �� ����� ���� ������ ��߰� ���ڰ� ������ �ȸ����� ��� ������ ����.