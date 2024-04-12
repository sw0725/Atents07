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
    {                   //�ι�° �ڸ��� ��Ʈ���� �鰡�����Ƿ� <int>�� ��������
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
