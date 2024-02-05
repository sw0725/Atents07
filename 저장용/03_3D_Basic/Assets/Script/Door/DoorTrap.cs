using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : DoorManual
{
    ParticleSystem ps;
    List<IAive> aives;

    protected override void Awake()
    {
        base.Awake();

        Transform child = transform.GetChild(3);
        ps = child.GetComponent<ParticleSystem>();
        aives = new List<IAive>(4);
    }

    protected override void OnOpen()
    {
        ps.Stop();
        ps.Play();

        foreach (IAive aive in aives) 
        {
            aive.Die();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        IAive aive = other.GetComponent<IAive>();
        if(aive != null) 
        {
            aives.Add(aive);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        IAive aive = other.GetComponent<IAive>();
        if (aive != null)
        {
            aives.Remove(aive);
        }
    }
}
