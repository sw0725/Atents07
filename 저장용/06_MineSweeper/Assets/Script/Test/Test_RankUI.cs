using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RankUI : TestBase
{
    public int aRecord;
    public float tRecord;
    public string rankerName = "Test";

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_RankSetting();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_ActionUpdate(aRecord, rankerName);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_TimeUpdate(tRecord, rankerName);
    }
}
