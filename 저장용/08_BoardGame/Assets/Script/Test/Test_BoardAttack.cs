using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_BoardAttack : Test_AutoDeploy
{
    protected override void Start()
    {
        base.Start();
        GetShip(ShipType.Carrier).onHit += (_)=> GameManager.Instance.CameraShake(1);
        GetShip(ShipType.BattleShip).onHit += (_)=> GameManager.Instance.CameraShake(1);
        GetShip(ShipType.Destroyer).onHit += (_)=> GameManager.Instance.CameraShake(1);
        GetShip(ShipType.Submarine).onHit += (_)=> GameManager.Instance.CameraShake(1);
        GetShip(ShipType.PatrolBoat).onHit += (_)=> GameManager.Instance.CameraShake(1);

        GetShip(ShipType.Carrier).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.BattleShip).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.Destroyer).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.Submarine).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.PatrolBoat).onSink += (_) => GameManager.Instance.CameraShake(3);

        board.onShipAttacked[ShipType.Carrier] += GetShip(ShipType.Carrier).OnHitted;
        board.onShipAttacked[ShipType.BattleShip] += GetShip(ShipType.BattleShip).OnHitted;
        board.onShipAttacked[ShipType.Destroyer] += GetShip(ShipType.Destroyer).OnHitted;
        board.onShipAttacked[ShipType.Submarine] += GetShip(ShipType.Submarine).OnHitted;
        board.onShipAttacked[ShipType.PatrolBoat] += GetShip(ShipType.PatrolBoat).OnHitted;
        AutoShipDeployment();
    }

    protected override void OnLClick(InputAction.CallbackContext context)
    {
        base.OnLClick(context);
        if(TargetShip == null) 
        {
            Vector2Int attackPos = board.GetMouseGridPosition();
            if (board.IsInBoard(attackPos) && board.IsAttackable(attackPos)) 
            {
                board.OnAttacked(attackPos);
            }
        }
    }

    Ship GetShip(ShipType shipType) 
    {
        return ships[(int)shipType - 1];
    }
}
