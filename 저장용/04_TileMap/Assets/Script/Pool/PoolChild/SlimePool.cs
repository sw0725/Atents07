using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : ObjectPool<Slime>
{
    protected override void OnGenerateObject(Slime comp)
    {
        comp.Pool = comp.transform.parent;
        comp.ShowPath(GameManager.Instance.showSlimePath); //경로세팅초기설정
    }
}
