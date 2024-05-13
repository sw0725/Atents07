using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishDeployButton : MonoBehaviour
{
    Button button;
    UserPlayer player;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        player = GameManager.Instance.User;
        foreach(var ship in player.Ships) 
        {
            ship.onDeploy += OnShipDeployed;
        }
    }

    private void OnShipDeployed(bool isDeployed)
    {
        if(isDeployed && player.IsAllDeployed) 
        {
            button.interactable = true;
        }
        else 
        {
            button.interactable= false;
        }
    }

    private void OnClick()
    {
        Debug.Log("전투씬 전환");
    }
}
