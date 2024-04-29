using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ShipDeploy : TestBase
{
    public Board board;
    public Ship ship;

    Ship[] ships;
    Ship TargetShip 
    {
        get => ship;
        set
        {
            ship = value;
            if (ship != null)                               //
            {
                ship.SetMaterialType(ship.IsDeployed);
            }                                               //
            ship?.gameObject.SetActive(true);
        }
    }

    private void Start() 
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
            ShipManager.Instance.SetDeployModeColor(board.IsShipDeploymentAvailable(ship, grid));//
            ship.transform.position = world;
        }
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
            Debug.Log($"배치성공 {TargetShip.gameObject.name}");
            TargetShip.SetMaterialType(true);//
            TargetShip = null;
        }
        else 
        {
            Debug.Log("배치 실패");
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
//배치가능하면 녹색 불가능하면 적색 배치완료시 노말 선택변경시 기존꺼 비활성화