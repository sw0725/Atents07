using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : Singltrun<ShipManager>
{
    public GameObject shipPrefab;
    public GameObject[] shipModels;
    public Material[] shipMaterials;

    public Material NormalShipMaterial => shipMaterials[0];
    public Material DepolyShipMaterial => shipMaterials[1];

    public int ShipTypeCount => shipTypeCount;
    int shipTypeCount;
    public int ShipDirectionCount => shipDirectionCount;
    int shipDirectionCount;

    readonly Color SuccessColor = new Color(0, 1, 0, 0.2f);
    readonly Color FailColor = new Color(1, 0, 0, 0.2f);
    readonly int BaseColor_ID = Shader.PropertyToID("_BaseColor");

    protected override void OnInitialize()
    {
        shipTypeCount = Enum.GetValues(typeof(ShipType)).Length -1;
        shipDirectionCount = Enum.GetValues(typeof(ShipDirection)).Length;
    }

    public Ship MakeShip(ShipType shipType, Transform ownerPlayer) 
    {
        GameObject shipObj = Instantiate(shipPrefab, ownerPlayer);
        GameObject modelPrefab = GetShipModel(shipType);
        Instantiate(modelPrefab, shipObj.transform);

        Ship ship = shipObj.GetComponent<Ship>();
        ship.Initialize(shipType);

        return ship;
    }

    GameObject GetShipModel(ShipType shipType) 
    {
        return shipModels[(int)shipType - 1];
    }

    public void SetDeployModeColor(bool isSuccess) 
    {
        if (isSuccess) 
        {
            DepolyShipMaterial.SetColor(BaseColor_ID, SuccessColor);
        }
        else 
        {
            DepolyShipMaterial.SetColor(BaseColor_ID, FailColor);
        }
    }
}
