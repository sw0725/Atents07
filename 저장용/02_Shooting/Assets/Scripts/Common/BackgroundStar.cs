using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStar : BackGround
{
    SpriteRenderer[] spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void MoveRight(int index)
    {
        base.MoveRight(index);
        int rand = Random.Range(0, 4);

        spriteRenderer[index].flipX = ((rand & 0b_01) != 0);
        spriteRenderer[index].flipX = ((rand & 0b_10) != 0);
    }
}
