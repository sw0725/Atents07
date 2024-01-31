using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuretStandard : TuretBase
{

    private void Start()
    {
        StartCoroutine(FireCoroutine);
    }

}
