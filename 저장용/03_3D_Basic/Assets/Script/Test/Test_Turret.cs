using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Turret : TestBase
{
    public float interval = 0.1f;

    Transform fireTransform;

    void Start()
    {
        fireTransform = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetBullet(fireTransform.position, 0.0f);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        StartCoroutine(FireContinue());
    }

    IEnumerator FireContinue() 
    {
        while (true) 
        {
            Factory.Instance.GetBullet(fireTransform.position, 0.0f);
            yield return new WaitForSeconds(interval);
        }
    }
}
