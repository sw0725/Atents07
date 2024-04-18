using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Log : TestBase
{
    public Logger logger;
    int count = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        action.Test.Enter.performed += OnEnter;
    }

    protected override void OnDisable()
    {
        action.Test.Enter.performed -= OnEnter;
        base.OnDisable();
    }

    private void OnEnter(InputAction.CallbackContext context)
    {
        logger.InputFieldFocusOn();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        logger.Log($"Test{count}");
        count++;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Color color = Color.red;
        string colorText = ColorUtility.ToHtmlStringRGB(color);
        logger.Log($"<#{colorText}>���� �ִ�</color>������ʹ� �⺻��");        //TMP�� ��� <#�����ڵ�>������</color>
    }
}
