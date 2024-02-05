using System.Collections;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    public float coolTime = 0.5f;
    float coolTimer = 0;
    public bool CanUse => coolTimer < 0.0f;

    TextMeshPro guid;
    bool doorOpen = false;

    protected override void Awake()
    {
        base.Awake();
        guid = GetComponentInChildren<TextMeshPro>(true);
    }

    private void Update()
    {
        coolTimer -= Time.deltaTime;
    }

    public virtual void Use()
    {
        if (CanUse) 
        {
            if (doorOpen)
            {
                Close();
                doorOpen = false;
            }
            else
            {
                Open();
                doorOpen = true;
            }
            coolTimer = coolTime;
        }
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 CameraForward = Camera.main.transform.forward;
            float angle = Vector3.SignedAngle(transform.forward, CameraForward, Vector3.up);

            if (angle > 90.0f)
            {
                guid.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);     //문의 회전에서 y축으로 반바퀴 더 돌리기
            }
            else 
            {
                guid.transform.rotation = transform.rotation;                                   //문의 회전 그대로 적용
            }

            guid.gameObject.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            guid.gameObject.SetActive(false);
        }
    }
}

//일정시간 이후 자동 닫힘
//트리거 안에 입성시 튜도리얼 - UI에서 추가말고 3D오브제로 추가
