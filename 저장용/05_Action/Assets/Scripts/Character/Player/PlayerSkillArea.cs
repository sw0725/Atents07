using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillArea : MonoBehaviour
{
    public float skillTick = 0.5f;      //nÃÊ´ç µ©
    public float skillPower = 0.2f;     //Æ½´ç nµ©(ÁõÆø)
    public float manaCost = 30.0f;
    public bool IsActivate => gameObject.activeSelf;

    float finalPower;
    float coolTime = 0.0f;

    List<Enemy> enemies = new List<Enemy>(4);
    IMana playerMana;

    private void Awake()
    {
        playerMana = GetComponentInParent<IMana>();
    }

    public void Activate(float power) 
    {
        coolTime = 0.0f;
        finalPower = power * (1 + skillPower);

        gameObject.SetActive(true);
    }

    public void Deactivate() 
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Remove(enemy);
            }
        }
    }

    private void Update()
    {
        coolTime -= Time.deltaTime;
        if (coolTime < 0) 
        {
            foreach (Enemy enemy in enemies) 
            {
                enemy.Defence(finalPower);
            }
            coolTime = skillTick;
        }
        playerMana.MP -= manaCost * Time.deltaTime;
    }
}
