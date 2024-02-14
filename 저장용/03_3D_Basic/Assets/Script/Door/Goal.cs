using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    Transform fire;
    private void Awake()
    {
        fire = transform.GetChild(2);    
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < fire.childCount; i++) 
        {
            ParticleSystem ps = fire.GetChild(i).GetComponent<ParticleSystem>();
            ps.Play();
        }
        Clear();
    }

    void Clear() 
    {
        GameManager.Instance.GameClear();
    }
}
