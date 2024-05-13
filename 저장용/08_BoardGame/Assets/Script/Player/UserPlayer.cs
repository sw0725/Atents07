using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    Ship selectedShip;
    protected Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            if (selectedShip != null && !selectedShip.IsDeployed)
            {
                selectedShip.gameObject.SetActive(false);
            }
            selectedShip = value;
            if (selectedShip != null && !selectedShip.IsDeployed)
            {
                selectedShip.SetMaterialType(false);
                selectedShip.transform.position = board.GridToWorld(board.GetMouseGridPosition());
                selectedShip.Rotate(false);
                SetSelectedShipColor();
                selectedShip.gameObject.SetActive(true);
            }
        }
    }

    public bool IsAllDeployed 
    {
        get 
        {
            bool result = true;
            foreach (var ship in Ships) 
            {
                if (!ship.IsDeployed)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }

    protected override void Start()
    {
        base.Start();

        opponent = gameManager.Enemy;
        gameManager.InputControler.onMouseClick += OnMouseClick;
        gameManager.InputControler.onMouseMove += OnMouseMove;
        gameManager.InputControler.onMouseWheel += OnMouseWheel;
    }

    public void SelectShipToDeploy(ShipType shipType) 
    {
        SelectedShip = Ships[(int)shipType - 1];
    }

    public void UndoShipDeploy(ShipType shipType) 
    {
        Board.UndoShipDeployment(Ships[(int)shipType - 1]);
    }

    void SetSelectedShipColor()
    {
        bool isSuccess = board.IsShipDeploymentAvailable(SelectedShip, SelectedShip.transform.position);
        shipManager.SetDeployModeColor(isSuccess);
    }

    private void OnMouseWheel(float delta)
    {
        if (gameManager.GameState == GameState.ShipDeploy)
        {
            if (SelectedShip != null)
            {
                SelectedShip.Rotate(delta < 0);
                SetSelectedShipColor();
            }
        }
    }

    private void OnMouseMove(Vector2 _)
    {
        if(gameManager.GameState == GameState.ShipDeploy) 
        {
            if (SelectedShip != null)
            {
                Vector2Int grid = board.GetMouseGridPosition();
                Vector3 world = board.GridToWorld(grid);
                SelectedShip.transform.position = world;
                SetSelectedShipColor();
            }
        }
    }

    private void OnMouseClick(Vector2 pos)
    {
        if (gameManager.GameState == GameState.ShipDeploy)//배잇-배치가능-배치-널 배없-위치배잇-배치취소
        {
            if (SelectedShip != null) 
            {
                if (board.ShipDeployment(SelectedShip, board.GetMouseGridPosition()))
                {
                    SelectedShip = null;
                }
                else
                {
                }
            }
            else 
            {
                Vector2Int grid = board.GetMouseGridPosition();
                ShipType shipType = board.GetShipTypeOnBoard(grid);
                if (shipType != ShipType.None)
                {
                    Ship ship = GetShip(shipType);
                    board.UndoShipDeployment(ship);
                }
                else 
                {
                }
            }
            
        }
        else if (gameManager.GameState == GameState.Battle)
        {
            Vector2Int grid = opponent.Board.GetMouseGridPosition();
            Attack(grid);
        }
    }

#if UNITY_EDITOR
    public void Test_BindInputSys()
    {
        gameManager.InputControler.onMouseClick += OnMouseClick;
        gameManager.InputControler.onMouseMove += OnMouseMove;
        gameManager.InputControler.onMouseWheel += OnMouseWheel;
    }
#endif
}
