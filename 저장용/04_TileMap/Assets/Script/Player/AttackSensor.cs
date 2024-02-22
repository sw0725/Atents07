using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    public bool go = false;

    List<Slime> slimes;

    void Attack()
    {
        foreach (Slime slime in slimes)
        {
            slime.TestDie();
        }
    }

    private void Update()
    {
        if (go) 
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if (slime != null) 
        {
            slime.ShowOutLine();
            slimes.Add(slime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if (slime != null)
        {
            slime.ShowOutLine(false);
            slimes.Remove(slime);
        }
    }
}
