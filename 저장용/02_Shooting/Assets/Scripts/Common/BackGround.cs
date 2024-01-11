using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float scrollingSpeed = 2.5f;
    const float BackgroundWidth = 13.5f;

    protected Transform[] bgSloats;

    float baseLineX;

    protected virtual void Awake()
    {
        bgSloats = new Transform[transform.childCount];
        for(int i = 0; i < bgSloats.Length; i++) 
        {
            bgSloats[i] = transform.GetChild(i);
        }

        baseLineX = transform.position.x - BackgroundWidth;
    }

    private void Update()
    {
        for(int i = 0;i < bgSloats.Length;i++) 
        {
            bgSloats[i].Translate(Time.deltaTime * scrollingSpeed * -transform.right);

            if (bgSloats[i].position.x < baseLineX) 
            {
                MoveRight(i);
            }
        }
    }

    protected virtual void MoveRight(int index) 
    {
        bgSloats[index].Translate(BackgroundWidth * bgSloats.Length *transform.right);
    }
}
