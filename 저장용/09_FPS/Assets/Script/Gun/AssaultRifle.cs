using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : GunBase
{
    protected override void FireProcess(bool isFireStart = true)    //�̰� �ѹ��ۿ� �ߵ����ϴϱ� �������ĺ��� ���Ӿ��� �ߵ����� ���𰡰� �ʿ� = �ڷ�ƾ
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
