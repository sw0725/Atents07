using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeTextPool : ObjectPool<DamegeText>
{
    public GameObject GetObject(int damege, Vector3? position)
    {
        DamegeText damegeText = GetObject(position);
        damegeText.SetDamege(damege);

        return damegeText.gameObject;
    }
}
