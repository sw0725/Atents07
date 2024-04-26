using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipType : byte 
{
    None =0,
    Carrier,    //항공모함(5)
    BattleShip, //전함(4)
    Destroyer,  //구축함(3)
    Submarine,  //잠수정(3)
    PatrolBoat  //경비정(2)
}

public enum ShipDirection : byte    //뱃머리가 바라보는 방향
{
    North =0,
    East,
    South,
    West
}

public class Ship : MonoBehaviour
{
    public ShipType ShipType 
    {
        get => shipType;
        set 
        {
            shipType = value;
            switch (shipType) 
            {
                case ShipType.Carrier:
                    size = 5;
                    shipName = "항공모함";
                    break;
                case ShipType.BattleShip:
                    size = 4;
                    shipName = "전함";
                    break;
                case ShipType.Destroyer:
                    size = 3;
                    shipName = "구축함";
                    break;
                case ShipType.Submarine:
                    size = 3;
                    shipName = "잠수함";
                    break;
                case ShipType.PatrolBoat:
                    size = 2;
                    shipName = "경비정";
                    break;
            }
        }
    }
    ShipType shipType = ShipType.None;

    public int HP 
    {
        get => hp;
        private set 
        {
            hp = value;
            if (hp < 1) 
            {
                OnSinking();
            }
        }
    }
    int hp = 0;
    bool IsAlive => hp > 0;

    public ShipDirection Direction 
    {
        get => direction;
        set 
        {
            direction = value;
            //modelRoot 방향 돌리기
        }
    }
    ShipDirection direction = ShipDirection.North;

    public string ShipName => shipName;
    string shipName = string.Empty;
    public int Size => size;
    int size = 0;
    public bool IsDeployed => isDeployed;
    bool isDeployed = false;                            //배가 배치됨?

    public Vector2Int[] Positions => positions;
    Vector2Int[] positions;                             //배가 차지하는 공간(그리드 좌표)

    public Action<bool> onDeploy;                       //배치/해제
    public Action<Ship> onHit;                          //공격당함(ship: 자기자신)
    public Action<Ship> onSink;                         //침몰함(ship: 자기자신)

    Transform modelRoot;
    Renderer shipRenderer;                              //마테리얼 바꿀것임

    public void Initialize(ShipType shipType)
    {
        ShipType = shipType;
        HP = Size;

        modelRoot = transform.GetChild(0);
        shipRenderer = modelRoot.GetComponentInChildren<Renderer>();

        ResetData();

        gameObject.name = $"{ShipType}_{Size}";
        gameObject.SetActive(false);
    }

    void ResetData()
    {
        Direction = ShipDirection.North;
        isDeployed = false;
        positions = null;
    }

    public void SetMaterialType(bool isNormal = true)       //true = 배치후/불투명 false = 배치전/반투명
    {
    
    }

    public void Deploy(Vector2Int[] deployPos) 
    {
    
    }

    public void UnDeploy() 
    {
    
    }

    public void Rotate(bool isCw = true)                   //true = 시계
    {
        
    }

    public void RandomRotate() 
    {
        
    }

    public void OnHitted() 
    {
            
    }

    void OnSinking() 
    { 
        
    }
}
