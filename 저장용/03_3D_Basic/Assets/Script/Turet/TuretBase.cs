using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TuretBase : MonoBehaviour
{
    public PoolObjectType bullet = PoolObjectType.Bullet;
    public float fireInterval = 1.0f;

    protected Transform fireTransform;
    protected Transform body;

    protected IEnumerator FireCoroutine;

    protected virtual void Awake()
    {
        body = transform.GetChild(2);
        fireTransform = body.GetChild(1);

        FireCoroutine = PeriodFire();
    }

    IEnumerator PeriodFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            Factory.Instance.GetObject(bullet, fireTransform.position, fireTransform.rotation.eulerAngles);
        }
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Vector3 P0 = transform.position;
        Vector3 P1 = transform.position + transform.forward * 2.0f;
        Vector3 P2 = P1 + Quaternion.AngleAxis(25.0f, transform.up) * (-transform.forward * 0.2f);
        Vector3 P3 = P1 + Quaternion.AngleAxis(-25.0f, transform.up) * (-transform.forward * 0.2f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(P0, P1);
        Gizmos.DrawLine(P1, P2);
        Gizmos.DrawLine(P1, P3);
    }
#endif
}
