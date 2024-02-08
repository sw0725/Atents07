using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapSlow : TrapBace
{
    public float slowDuration = 5.0f;
    [Range(0.1f, 2.0f)]
    public float slowRatio = 0.5f;  //50ÆÛ

    ParticleSystem ps;

    private void Awake()
    {
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject gameObject)
    {
        ps.Play();
        Player player = gameObject.GetComponent<Player>();
        if (player != null) 
        {
            player.SetSpeedModifier(slowRatio);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            StopAllCoroutines();
            StartCoroutine(Restore(player));
        }
    }

    IEnumerator Restore(Player player)
    {
        yield return new WaitForSeconds(slowDuration);
        player.RestoreMoveSpeed();
    }
}
