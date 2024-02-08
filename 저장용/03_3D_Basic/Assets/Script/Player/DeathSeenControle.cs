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
    //onDie받아 처리 플레이어와 같은 위치로 가상 카메라와 트랙 카트를 자식으로 둔다플레이어 사망시 카트 움직
    //자식으로 둔 가상카메라의 우선순위 변동 이는 플레이어를 바라본다.
}
