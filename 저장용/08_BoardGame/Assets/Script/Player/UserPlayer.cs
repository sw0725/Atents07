using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    protected override void Start()
    {
        base.Start();

        opponent = gameManager.Enemy;
    }

    public void UndoShipDeploy(ShipType shipType) 
    {
        Board.UndoShipDeployment(ships[(int)shipType - 1]);
    }
}
