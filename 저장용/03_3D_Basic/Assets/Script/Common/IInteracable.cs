using System.Collections;
using System.Collections.Generic;
using UnityEngine;
                                        //�������̽��� ��ӿ� ������ ����
public interface IInteracable           //�������̽��� �⺻������ ���� public
{                                       //��������� ���ԺҰ�, �� ����� ����
    bool CanUse                        //��������� Ȯ���ϴ� ������Ƽ 
    {
        get;
    }

    void Use();                         //����Լ��� ���� ����
}
