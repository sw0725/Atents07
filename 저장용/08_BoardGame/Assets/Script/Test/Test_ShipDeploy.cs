using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ShipDeploy : TestBase
{
    public Board board;
    Ship ship;

    protected Ship[] ships;
    Ship TargetShip 
    {
        get => ship;
        set
        {
            if(ship != null && !ship.IsDeployed) 
            {
                ship.gameObject.SetActive(false);
            }
            ship = value;
            if (ship != null && !ship.IsDeployed)
            {
                ship.SetMaterialType(false);
                ship.transform.position = board.GridToWorld(board.GetMouseGridPosition());
                OnShipMovemant();
                ship.gameObject.SetActive(true);
            }
        }
    }

    protected virtual void Start() 
    {
        ships = new Ship[ShipManager.Instance.ShipTypeCount];
        ships[(int)ShipType.Carrier - 1] = ShipManager.Instance.MakeShip(ShipType.Carrier, transform);
        ships[(int)ShipType.BattleShip - 1] = ShipManager.Instance.MakeShip(ShipType.BattleShip, transform);
        ships[(int)ShipType.Destroyer - 1] = ShipManager.Instance.MakeShip(ShipType.Destroyer, transform);
        ships[(int)ShipType.Submarine - 1] = ShipManager.Instance.MakeShip(ShipType.Submarine, transform);
        ships[(int)ShipType.PatrolBoat - 1] = ShipManager.Instance.MakeShip(ShipType.PatrolBoat, transform);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        action.Test.MouseMove.performed += OnMouseMove;
        action.Test.MouseWheel.performed += OnMouseWheel;
    }

    protected override void OnDisable()
    {
        action.Test.MouseMove.performed -= OnMouseMove;
        action.Test.MouseWheel.performed -= OnMouseWheel;
        base.OnDisable();
    }

    //private void Start()
    //{
    //    ship.Initialize(ShipType.Carrier);
    //    ship.gameObject.SetActive(true);
    //}

    private void OnMouseWheel(InputAction.CallbackContext context)
    {
        if(ship != null)
        {
            float wheel = context.ReadValue<float>();
            if (wheel > 0)
            {
                ship.Rotate(false);
            }
            else
            {
                ship.Rotate(true);
            }
        }
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (ship != null)
        {
            Vector2Int grid = board.GetMouseGridPosition();
            Vector3 world = board.GridToWorld(grid);
            ship.transform.position = world;
            OnShipMovemant();
        }
    }

    void OnShipMovemant()
    {
        bool isSuccess = board.IsShipDeploymentAvailable(TargetShip, TargetShip.transform.position);
        ShipManager.Instance.SetDeployModeColor(isSuccess);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        TargetShip = ships[(int)ShipType.Carrier - 1];
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        TargetShip = ships[(int)ShipType.BattleShip - 1];
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        TargetShip = ships[(int)ShipType.Destroyer - 1];
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        TargetShip = ships[(int)ShipType.Submarine - 1];
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        TargetShip = ships[(int)ShipType.PatrolBoat - 1];
    }

    protected override void OnLClick(InputAction.CallbackContext context)
    {
        if (TargetShip != null && board.ShipDeployment(TargetShip, board.GetMouseGridPosition()))
        {
            Debug.Log($"��ġ���� {TargetShip.gameObject.name}");
            TargetShip = null;
        }
        else 
        {
            //Debug.Log("��ġ ����");
        }
    }
    protected override void OnRClick(InputAction.CallbackContext context)
    {
        Vector2Int grid = board.GetMouseGridPosition();
        ShipType shipType = board.GetShipTypeOnBoard(grid);
        if (shipType != ShipType.None) 
        {
            Ship ship = ships[(int)shipType - 1];
            board.UndoShipDeployment(ship);
        }
    }
}