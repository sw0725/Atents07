using System.Collections;
using TMPro;
using UnityEngine;

class DoorManualAutoClose : DoorManual
{
    public float closetimer = 3.0f;

    public new void Use()       //부모 함수 무시 = new
    {
        Open();
        StopAllCoroutines();
        StartCoroutine(CloseTimer());
    }

    IEnumerator CloseTimer() 
    {
        yield return new WaitForSeconds(closetimer);
        Close();
    }
}

