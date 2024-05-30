using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : GunBase
{
    protected override void FireProcess(bool isFireStart = true)    //이건 한번밖에 발동안하니까 누른직후부터 끊임없이 발동해줄 무언가가 필요 = 코루틴
    {
        if (isFireStart)
        {
            StartCoroutine(FireRepeat());
        }
        else 
        {
            StopAllCoroutines();
            isFireReady = true;
        }
    }

    IEnumerator FireRepeat() 
    {
        while (BulletCount > 0) 
        {
            MuzzleEffectOn();
            BulletCount--;

            HitProcess();
            FireRecoil();
            yield return new WaitForSeconds(1/fireRate);
        }
        isFireReady = true;
    }
}
