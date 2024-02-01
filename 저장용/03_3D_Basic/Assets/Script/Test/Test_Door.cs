using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Test_Door : TestBase
{
    public TextMeshPro text;
    public DoorBase door;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Vector3 CameraForward = Camera.main.transform.forward;
        float angle = Vector3.SignedAngle(transform.forward, CameraForward, Vector3.up);
        Debug.Log(angle);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        float angle = Vector3.Angle(door.transform.forward, cameraForward);
        Debug.Log(angle);
    }
}
