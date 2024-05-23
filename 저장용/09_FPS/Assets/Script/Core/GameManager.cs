using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    public Player Player => player;
    Player player;

    public CinemachineVirtualCamera FollowCamera => followCamera;
    CinemachineVirtualCamera followCamera;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();

        GameObject obj = GameObject.FindWithTag("FollowCamera");
        if (obj != null) 
        {
            followCamera = obj.GetComponent<CinemachineVirtualCamera>();
        }
    }
}
