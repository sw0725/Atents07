using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployPanel : MonoBehaviour
{
    DeployToggle[] toggles;

    private void Awake()
    {
        toggles = GetComponentsInChildren<DeployToggle>();
        foreach (DeployToggle toggle in toggles) 
        {
            toggle.onSelect += UnSelectOthers;
        }
    }

    private void UnSelectOthers(DeployToggle self)
    {
        foreach (DeployToggle toggle in toggles) 
        {
            if(toggle != self) 
            {
                toggle.SetNotSelect();
            }
        }
    }
}
