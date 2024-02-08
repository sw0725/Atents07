using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSeenControle : MonoBehaviour
{
    public float cartSpeed = 20.0f;

    CinemachineVirtualCamera vCam;
    CinemachineDollyCart cart;
    Player player;

    private void Awake()
    {
        cart = transform.GetChild(1).GetComponent<CinemachineDollyCart>();
        vCam = cart.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.OnDie += DeathSceneStart;
    }

    void DeathSceneStart() 
    {
        vCam.Priority = 100;
        cart.m_Speed = cartSpeed;
    }

    private void Update()
    {
        transform.position = player.transform.position;
    }
    //onDie�޾� ó�� �÷��̾�� ���� ��ġ�� ���� ī�޶�� Ʈ�� īƮ�� �ڽ����� �д��÷��̾� ����� īƮ ����
    //�ڽ����� �� ����ī�޶��� �켱���� ���� �̴� �÷��̾ �ٶ󺻴�.
}
