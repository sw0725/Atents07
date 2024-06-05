using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : RecycleObject
{
    public float spinSpeed = 360.0f;    //초당 360도 회전

    Transform meshTransform;

    private void Awake()
    {
        meshTransform = transform.GetChild(0);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(30));
    }

    private void Update()
    {
        meshTransform.Rotate(Time.deltaTime * spinSpeed * Vector3.up, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if(player != null) 
            {
                OnItemConsum(player);
                gameObject.SetActive(false);
            }
        }
    }

    protected virtual void OnItemConsum(Player player) 
    {
        
    }
}
