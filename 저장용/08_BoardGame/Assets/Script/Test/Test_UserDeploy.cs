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
//1.유저플레이어에 함선 배치기능 추가 -> 쉽 드플로이 먼트 추가 토글 버튼이 셀렉트상태가 되면 함선 배치 가능, 배치 완료시 버튼이 디플로이 상태 전환
