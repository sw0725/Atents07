using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunBase : MonoBehaviour
{
    public float range;
    public int clipSize;
    public float damege;
    public float fireRate;
    public float spread;
    public float recoil;

    public Action<int> onBulletCountChange; //���� �Ѿ� ����
    public Action<float> onFire;            //�ݵ�

    protected Transform fireTransform;        //�÷��̾� ī�޶� ��ġ
    protected bool isFireReady = true;
    protected int BulletCount
    {
        get => bulletCount;
        set
        {
            bulletCount = value;
            onBulletCountChange?.Invoke(bulletCount);
        }
    }
    int bulletCount;

    readonly int onFireID = Shader.PropertyToID("OnFire");      //muzzle����Ʈ �ߵ���

    VisualEffect muzzleEffect;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
    }

    private void Initialize()
    {
        BulletCount = clipSize;
        isFireReady = true;
    }

    void Fire()
    {
        if (isFireReady && BulletCount > 0)
        {
            FireProcess();
        }
    }

    protected virtual void FireProcess()
    {
        isFireReady = false;
        MuzzleEffectOn();
        BulletCount--;
        StartCoroutine(FireReady());
    }

    IEnumerator FireReady () 
    {
        yield return new WaitForSeconds(1/fireRate);
        isFireReady = true;
    }

    protected void MuzzleEffectOn()
    {
        muzzleEffect.SendEvent(onFireID);
    }

    protected void HitProcess() //���̺ε��� ���� ���� ó��
    {
        
    }

    protected void FireRecoil() 
    {
        onFire?.Invoke(recoil);
    }

    public void Equip()
    {
        fireTransform = GameManager.Instance.Player.FireTransform;
        Initialize();
    }

    public void UnEquip()
    {
        StopAllCoroutines();
        isFireReady = true;
    }

    protected Vector3 GetFireDirection() 
    {
        Vector3 result = fireTransform.forward;
        result = Quaternion.AngleAxis(UnityEngine.Random.Range(-spread, spread), fireTransform.right) * result;     //x �� ����
        result = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), fireTransform.forward) * result;            //z �� ����
        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(fireTransform != null) 
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(fireTransform.position, fireTransform.position + fireTransform.forward * range);
        }
    }
#endif
}
