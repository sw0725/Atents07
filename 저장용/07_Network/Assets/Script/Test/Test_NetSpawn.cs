using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_NetSpawn : TestBase
{
    public GameObject Orb;
    Transform firePos;

    private void Start()
    {
        firePos = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(Orb, firePos.position, firePos.rotation);
    }
}
