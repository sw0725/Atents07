using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployment : MonoBehaviour
{
    private void Start()
    {
        GameManager manager = GameManager.Instance;
        manager.GameState = GameState.ShipDeploy;
        manager.User.BindInputSys();

        CinemachineVirtualCamera vcam = manager.GetComponentInChildren<CinemachineVirtualCamera>();
        vcam.m_Lens.OrthographicSize = 7.0f;

        manager.TurnManager.TurnManagerStop();
    }
}
