using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_UserDeploy : TestBase
{
    public DeployToggle toggle;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(0);
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(1);
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(2);
    }
}
//1.�����÷��̾ �Լ� ��ġ��� �߰� -> �� ���÷��� ��Ʈ �߰� ��� ��ư�� ����Ʈ���°� �Ǹ� �Լ� ��ġ ����, ��ġ �Ϸ�� ��ư�� ���÷��� ���� ��ȯ
