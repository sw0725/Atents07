using System.Collections;
using System.Collections.Generic;
using UnityEngine;
                                        //인터페이스는 상속에 제한이 없다
public interface IInteracable           //인터페이스는 기본적으로 전부 public
{                                       //멤버변수는 포함불가, 단 상수는 가능
    bool CanUse                        //사용중인지 확인하는 프로퍼티 
    {
        get;
    }

    void Use();                         //멤버함수는 선언만 가능
}
