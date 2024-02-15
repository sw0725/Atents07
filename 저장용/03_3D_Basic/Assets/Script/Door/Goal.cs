using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    ParticleSystem[] fireworks;

    private void Awake()
    {
        Transform fire = transform.GetChild(2);
        fireworks = new ParticleSystem[fire.childCount];

        for (int i = 0; i < fireworks.Length; i++) 
        {
            fireworks[i] = fire.GetChild(i).GetComponent<ParticleSystem>();
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            OnGoalIn();
            StartCoroutine(FireWorkEffect());
            StartCoroutine(GameClaer());
        }
    }

    IEnumerator GameClaer() 
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.GameClear();
    }

    IEnumerator FireWorkEffect() 
    {
        for (int i = fireworks.Length - 1; i > -1; i--) 
        {
            int index = Random.Range(0, i);

            (fireworks[index], fireworks[i]) = (fireworks[i], fireworks[index]);        //c#스왑기능
        }

        for (int i = 0; i < fireworks.Length; i++) 
        {
            yield return new WaitForSeconds(0.5f);
            fireworks[i].Play();
        }
    }

    private void OnGoalIn()
    {
        foreach (var fire in fireworks) 
        {
            fire.Play();
        }
    }
}
