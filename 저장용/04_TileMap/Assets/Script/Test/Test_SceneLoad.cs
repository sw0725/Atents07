using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_SceneLoad : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {       //�������� �ҷ��� ���� �ƹ��͵� ����       =>  �ε�â�� �����ֱ� ���ؼ��� �񵿱� ������� �ε��ؾ���
        SceneManager.LoadScene("LoadSampleScene");      //������ �̸����� �ҷ��ü� �ִ�
        //SceneManager.LoadScene(0);                    //����ȣ�ε� ����
    }                                                   //������(Synchronous) ���ڵ尡 ������ �ٸ��ڵ����
}
