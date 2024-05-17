using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.GameState = GameState.Battle;

        gameManager.User.BindInputSys();

        CinemachineVirtualCamera vCam = gameManager.GetComponentInChildren<CinemachineVirtualCamera>();
        vCam.m_Lens.OrthographicSize = 10.0f;

        if (!gameManager.LoadShipDeployData()) 
        {
            gameManager.User.AutoShipDeployment(true);
        }
        gameManager.Enemy.AutoShipDeployment(gameManager.IsTestMode);
    }
}
