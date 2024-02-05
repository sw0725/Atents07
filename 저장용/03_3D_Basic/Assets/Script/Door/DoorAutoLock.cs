using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoLock : DoorAuto
{
    public Color lockColor = new Color(1,0,0,0.3f); 
    public Color unlockColor = new Color(0,0,1,0.3f);

    bool locking;

    bool Locking 
    {
        get => locking;
        set 
        {
            if (locking != value) 
            {
                locking = value;
                if (locking)
                {
                    material.color = lockColor;
                    sensor.enabled = false;
                }
                else 
                {
                    material.color = unlockColor;
                    sensor.enabled = true;
                }
            }
        }
    }

    BoxCollider sensor;
    Material material;

    protected override void Awake()
    {
        base.Awake();
        sensor = GetComponent<BoxCollider>();

        Transform door = transform.GetChild(1);
        door = door.GetChild(0);

        MeshRenderer mesh = door.GetComponent<MeshRenderer>();      //���׸����� �޽������� �ȿ� ���ִ�
        material = mesh.material;                                   //���׸����� ������ �鰥�� ������ ������ ���ſ�����
    }

    protected override void Start()
    {
        base.Start();
        Locking = true;
    }

    protected override void OnKeyUse()
    {
        Locking = false;
    }
}
