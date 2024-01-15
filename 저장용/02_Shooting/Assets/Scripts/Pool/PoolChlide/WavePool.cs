using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WavePool : ObjectPool<Wave>
{
    protected override void OnGetObject(Wave component)
    {
        component.SetStartPotition(component.transform.position);
    }
}
