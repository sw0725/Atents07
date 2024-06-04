using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reloadDuration = 1.0f;
    bool isReloading = false;

    protected override void FireProcess(bool isFireStart = true)
    {
        if(isFireStart)
        {
            base.FireProcess(isFireStart);
            HitProcess();
            FireRecoil();
        }
    }

    public void Reload ()
    {
        if (!isReloading) 
        {
            StopAllCoroutines();    //FireProcess 실행시키는 코루틴으로 isFireReady가 true되는것 방지
            isReloading = true;
            isFireReady = false;
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading() 
    {
        yield return new WaitForSeconds(reloadDuration);
        isFireReady= true;
        BulletCount = clipSize;
        isReloading = false;
    }
}
