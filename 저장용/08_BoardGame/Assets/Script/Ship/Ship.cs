using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipType : byte 
{
    None =0,
    Carrier,    //�װ�����(5)
    BattleShip, //����(4)
    Destroyer,  //������(3)
    Submarine,  //�����(3)
    PatrolBoat  //�����(2)
}

public enum ShipDirection : byte    //��Ӹ��� �ٶ󺸴� ����
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
                    shipName = "�װ�����";
                    break;
                case ShipType.BattleShip:
                    size = 4;
                    shipName = "����";
                    break;
                case ShipType.Destroyer:
                    size = 3;
                    shipName = "������";
                    break;
                case ShipType.Submarine:
                    size = 3;
                    shipName = "�����";
                    break;
                case ShipType.PatrolBoat:
                    size = 2;
                    shipName = "�����";
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
            //modelRoot ���� ������
        }
    }
    ShipDirection direction = ShipDirection.North;

    public string ShipName => shipName;
    string shipName = string.Empty;
    public int Size => size;
    int size = 0;
    public bool IsDeployed => isDeployed;
    bool isDeployed = false;                            //�谡 ��ġ��?

    public Vector2Int[] Positions => positions;
    Vector2Int[] positions;                             //�谡 �����ϴ� ����(�׸��� ��ǥ)

    public Action<bool> onDeploy;                       //��ġ/����
    public Action<Ship> onHit;                          //���ݴ���(ship: �ڱ��ڽ�)
    public Action<Ship> onSink;                         //ħ����(ship: �ڱ��ڽ�)

    Transform modelRoot;
    Renderer shipRenderer;                              //���׸��� �ٲܰ���

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

    public void SetMaterialType(bool isNormal = true)       //true = ��ġ��/������ false = ��ġ��/������
    {
    
    }

    public void Deploy(Vector2Int[] deployPos) 
    {
    
    }

    public void UnDeploy() 
    {
    
    }

    public void Rotate(bool isCw = true)                   //true = �ð�
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
