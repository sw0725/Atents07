using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : RecycleObject
{
    public float moveSpeed = 2.0f;
    public float dirChangeinterval = 1.0f;
    public int dirChangeCountMax = 5;
    
    Vector3 dir;
    Transform player;
    Animator animator;
    int dirChangeCount = 5;

    int DirChageCount 
    {
        get { return dirChangeCount; }
        set 
        {
            dirChangeCount = value;
            animator.SetInteger("Count", dirChangeCount);
            StopAllCoroutines();

            if (dirChangeCount > 0 && gameObject.activeSelf) 
            {
                StartCoroutine(DirectionChange()); ;
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StopAllCoroutines();

        DirChageCount = dirChangeCountMax;
        player = GameManager.Instance.Player.transform;
        dir = Vector3.zero;
    }

    IEnumerator DirectionChange() 
    {
        while (true)
        {
            yield return new WaitForSeconds(dirChangeinterval);
            if (Random.value < 0.4f)
            {
                Vector2 playerToPowerup = transform.position - player.position;
                dir = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * playerToPowerup;
            }
            else 
            {
                dir = Random.insideUnitCircle;
            }
            dir.Normalize();
            DirChageCount--;
        }
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DirChageCount>0 && collision.gameObject.CompareTag("Border")) 
        {
            dir = Vector3.Reflect(dir, collision.contacts[0].normal);
            DirChageCount--;
        }
    }
}
