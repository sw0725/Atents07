using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManualKeyUse : DoorBase
{
    protected override void OnKeyUse()
    {
        Open();
    }
}
