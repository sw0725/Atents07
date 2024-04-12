using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Rank : TestBase
{
    public int rank;
    public int aRecord;
    public int tRecord;
    public string testName = "tester";

    public RankLine rankLine1;
    public RankLine rankLine2;

    protected override void OnTest1(InputAction.CallbackContext context)
    {                   //두번째 자리에 인트값이 들가있으므로 <int>는 생략가능
        rankLine1.SetData<int>(rank, aRecord, testName);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        rankLine2.SetData(rank, tRecord, testName);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        rankLine1.ClearLine();
        rankLine2.ClearLine();
    }
}
