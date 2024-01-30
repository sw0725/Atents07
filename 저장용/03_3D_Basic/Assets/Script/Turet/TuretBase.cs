using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuretBase : MonoBehaviour
{
    public PoolObjectType bullet = PoolObjectType.Bullet;
    public float fireInterval = 1.0f;

    bool isFire = false;

    Transform fireTransform;

    IEnumerator FireCoroutine;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(2);
        fireTransform = child.GetChild(1);

        FireCoroutine = PeriodFire();
    }


    protected void StartFire()
    {
        if (!isFire)
        {
            StartCoroutine(FireCoroutine);
            isFire = true;
        }
    }

    protected void StopFire()
    {
        if (isFire)
        {
            StopCoroutine(FireCoroutine);
            isFire = false;
        }
    }

    IEnumerator PeriodFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            Factory.Instance.GetObject(bullet, fireTransform.position, fireTransform.rotation.eulerAngles);
        }
    }
}
