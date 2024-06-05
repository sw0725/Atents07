using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum GunType : byte 
{
    Revolver = 0,
    ShotGun,
    AssaultRifle
}

public class GunBase : MonoBehaviour
{
    public float range;
    public int clipSize;
    public float damege;
    public float fireRate;
    public float spread;
    public float recoil;

    public Action<int> onBulletCountChange; //���� �Ѿ� ����
    public Action onAmmoDepleted; //���� �Ѿ� ����
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

            if(bulletCount < 1) 
            {
                onAmmoDepleted?.Invoke();
            }
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

    public void Fire(bool isFireStart = true)
    {
        if (isFireReady && BulletCount > 0)
        {
            FireProcess(isFireStart);
        }
    }

    protected virtual void FireProcess(bool isFireStart = true)         //�߻��Է��� ���� = true
    {
        isFireReady = false;
        MuzzleEffectOn();
        StartCoroutine(FireReady());
        BulletCount--;
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
        Ray ray = new(fireTransform.position, GetFireDirection());
        if (Physics.Raycast(ray, out RaycastHit hitInfo, range)) 
        {
            Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
            Factory.Instance.GetBulletHole(hitInfo.point, hitInfo.normal, reflect);     //�븻���� : Ư������ �Ѹ鿡 ������ ����, ��� ������ �������� ���� �� �ִ�
        }
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

    public void TestFire (bool isFireStart = true)
    {
        if( fireTransform == null )
        {
            Equip();
        }
        Fire(isFireStart);
    }
#endif
}
