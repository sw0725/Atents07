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

    SphereCollider sightTrigger;
    Player target;
    Transform body;

    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
        body = transform.GetChild(2);
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
        if (other.transform == GameManager.Instance.Player.transform)       //�̱��Ͽ��� ������ ��ü�̹Ƿ� �� �ϳ����̴�. �� �񱳰���
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
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
            //������� - �κ��Ͱ� ���� ���� ���̰� ��ȯ |SignedAngle-�κ��Ͱ� ���̰��� �������� ��ȯ(�������� ����)
            float angle = Vector3.Angle(body.forward, dir);
            if (angle < fireAngle)
            {
                isFireStart = true;
            }
        }

        if (isFireStart)
        {
            StartFire();
        }
        else 
        {
            StopFire();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 0.3f);
        if (body == null) 
        {
            body = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + body.transform.forward * sightRange;
        Gizmos.color = Color.yellow;
        Handles.DrawDottedLine(from, to, 2.0f);

        Handles.color = Color.red;
        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * body.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * body.forward;

        to= transform.position + dir1 * sightRange;
        Handles.DrawLine(from, to);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from, to);
    }
#endif
}

///�������ͷ������
///�÷��̾ �����Ÿ��� ���ٽ� �÷��̾� �������� ��ü ȸ��(y�ุ)
///�÷��̾ �ͷ��� �߻簢�ȿ� ������ �Ѿ��� �ֱ������� �߻� ����� �������
///����������� �þ߹����� �߻簢 �׸���(Handles ��õ)
