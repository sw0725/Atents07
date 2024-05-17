using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_LoadSave : TestBase
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameState = GameState.ShipDeploy;
        gameManager.User.BindInputSys();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if (gameManager.SaveShipDeployData()) 
        {
            Debug.Log("����");
        }
        else
        {
            Debug.Log("����");
        }
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        if (gameManager.LoadShipDeployData())
        {
            Debug.Log("����");
        }
        else
        {
            Debug.Log("����");
        }
    }
}
