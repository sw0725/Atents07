using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : CounterBase
{
    private void Start()
    {
        GameManager.Instance.onFlagCountChange += Refreash;
        Refreash(GameManager.Instance.FlagCount);
    }

}
