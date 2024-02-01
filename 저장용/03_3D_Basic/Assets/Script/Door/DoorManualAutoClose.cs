using System.Collections;
using TMPro;
using UnityEngine;

class DoorManualAutoClose : DoorManual
{
    public float closetimer = 3.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Use()
    {
        base.Use();
        StopAllCoroutines();
        StartCoroutine(CloseTimer());
    }

    IEnumerator CloseTimer() 
    {
        yield return new WaitForSeconds(closetimer);
        Close();
    }
}

//일정시간 이후 자동 닫힘
//트리거 안에 입성시 튜도리얼 - UI에서 추가말고 3D오브제로 추가
