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
