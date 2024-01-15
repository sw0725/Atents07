using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMiniPool : ObjectPool<AstroidMini>
{
    protected override void OnGetObject(AstroidMini component)
    {
        component.Direction = -component.transform.right;
    }
}
