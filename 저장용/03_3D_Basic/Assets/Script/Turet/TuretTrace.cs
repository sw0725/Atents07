using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TuretTrace : TuretBase
{
    public float sightRange = 10.0f;
    public float turnSpeed = 2.0f;
    public float fireAngle = 10.0f;

    bool isFire = false;
#if UNITY_EDITOR
    bool IsRedState => isFire;
    bool IsOrengeState => IsTargetVisible;
    bool IsTargetVisible = false;
#endif

    SphereCollider sightTrigger;
    Player target;

    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        sightTrigger.radius= sightRange;
    }

    private void Update()
    {
        LookTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)       //�̱��Ͽ��� ������ ��ü�̹Ƿ� �� �ϳ����̴�. �� �񱳰���
        {
            target = GameManager.Instance.Player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform) 
        {
            target = null;
        }
    }

    private void LookTarget() 
    {
        bool isFireStart = false;
        if (target != null)
        {                                                                                                //body�� ���͸� dir�� ��ü
            Vector3 dir = target.transform.position - transform.position;   //��ǥ������ ���⺤�� ����     //body.forward = dir->���
            dir.y = 0.0f;                                               //��ǥ���ͱ����� ���� ����

            if (isVisibleTarget(dir))
            {
                body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
                //������� - �κ��Ͱ� ���� ���� ���̰� ��ȯ |SignedAngle-�κ��Ͱ� ���̰��� �������� ��ȯ(�������� ����)
                float angle = Vector3.Angle(body.forward, dir);
                if (angle < fireAngle)
                {
                    isFireStart = true;
                }
            }

        }
#if UNITY_EDITOR
        else 
        {
            IsTargetVisible = false;
        }
#endif

        if (isFireStart)
        {
            StartFire();
        }
        else 
        {
            StopFire();
        }
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

    private bool isVisibleTarget(Vector3 lookDirection) 
    {
        bool result = false;
                                                            //out=������� -> �̷��Ķ���ʹ� �ʱ�ȭ �� �ʿ���� �Լ������ ���� �ڵ� ������ 
        Ray ray = new Ray(body.position, lookDirection);    //ref=���(����)-> �������� �Ķ���ͷ� ���� �ش簪 ������ ���� �����
        if (Physics.Raycast(ray, out RaycastHit hitInfo, sightRange))  
        {                                                   //���̾�� �Ѿ��� ���̾ Ignore Raycast�� ����->�Ѿ��� ���� ���� ���� ����
            if (hitInfo.transform == target.transform)      //Ȥ�� LayerMask.GetMask("Player") �� ���� Ư�� ���̾ �����ϰ� �� �� �ִ�
            {
                result = true;
            }
        }
#if UNITY_EDITOR
        IsTargetVisible = result;
#endif
        return result;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 0.3f);
        if (body == null) 
        {
            body = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + body.transform.forward * sightRange;
        Gizmos.color = Color.yellow;
        Handles.DrawDottedLine(from, to, 2.0f);

        Handles.color = Color.green;
        if (IsRedState)
        {
            Handles.color = Color.red;
        }
        else if (IsOrengeState) 
        {                                                       //���ø޸�-����ƽ����
            Handles.color = new Color(1.0f, 0.5f, 0.0f); //�÷��� ��Ÿ�� �󸶵��� ���ص� �������
        }

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * body.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * body.forward;

        to= transform.position + dir1 * sightRange;
        Handles.DrawLine(from, to);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from, to);
        Handles.DrawWireArc(from, transform.up, dir1, fireAngle * 2.0f, sightRange);
    }               //���߾�, �׸��� ����, ���۰�, ���۰����� ���ᰢ, ������
#endif
}

///�������ͷ������
///�÷��̾ �����Ÿ��� ���ٽ� �÷��̾� �������� ��ü ȸ��(y�ุ)
///�÷��̾ �ͷ��� �߻簢�ȿ� ������ �Ѿ��� �ֱ������� �߻� ����� �������
///����������� �þ߹����� �߻簢 �׸���(Handles ��õ)
