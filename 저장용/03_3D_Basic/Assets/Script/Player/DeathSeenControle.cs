using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSeenControle : MonoBehaviour
{
    public CinemachineVirtualCamera playerCam;

    CinemachineVirtualCamera vCam;
    CinemachineDollyCart cart;

    private void Awake()
    {
        cart = transform.GetChild(1).GetComponent<CinemachineDollyCart>();
        vCam = cart.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        GameManager.Instance.Player.OnDie += DeathScene;
    }

    void DeathScene() 
    {
        transform.position = GameManager.Instance.Player.transform.position;
        playerCam.Priority = 10;
        vCam.Priority = 100;
        cart.m_Speed = 10;
    }
    //onDie�޾� ó�� �÷��̾�� ���� ��ġ�� ���� ī�޶�� Ʈ�� īƮ�� �ڽ����� �д��÷��̾� ����� īƮ ����
    //�ڽ����� �� ����ī�޶��� �켱���� ���� �̴� �÷��̾ �ٶ󺻴�.
}
