using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStar : BackGround
{
    protected override void MoveRight(int index)
    {
        base.MoveRight(index);
        SpriteRenderer spriteRenderer = bgSloats[index].gameObject.GetComponent<SpriteRenderer>();
        int ran = Random.Range(0, 3);
        switch (ran) 
        {
            case 0:
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = true;
                break;
            case 1:
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
                break;
            case 2:
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = true;
                break;
            case 3:
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
                break;
        }
    }
}
