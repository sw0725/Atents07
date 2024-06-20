using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapCamera : MonoBehaviour
{
    public float zoomMin = 7;
    public float zoomMax = 15;
    float zoomTarget = 7.0f;

    public float smooth = 2.0f;

    Vector3 offset;
    Transform target;
    Camera cam;

    PlayerInputAction uiAction;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        uiAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        uiAction.UI.Enable();
        uiAction.UI.MiniMapZoomIn.performed += OnZoomIn;
        uiAction.UI.MiniMapZoomOut.performed += OnZoomOut;
    }

    private void OnDisable()
    {
        uiAction.UI.MiniMapZoomIn.performed -= OnZoomIn;
        uiAction.UI.MiniMapZoomOut.performed -= OnZoomOut;
        uiAction.UI.Disable();
    }

    private void Start()
    {
        zoomTarget = zoomMin;
    }

    public void Initialize(Player player)
    {
        offset = transform.position;
        target = player.transform;
        transform.position = target.position + offset;
        player.onSpawn += () =>
        {
            transform.position = target.position;
            transform.rotation = Quaternion.Euler(90, target.eulerAngles.y, 0);
        };
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smooth);
        transform.rotation = Quaternion.Euler(90, target.eulerAngles.y, 0);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, Time.deltaTime);
    }


    private void OnZoomIn(InputAction.CallbackContext context)
    {
        zoomTarget -= 1.0f;
        zoomTarget = Mathf.Clamp(zoomTarget, zoomMin, zoomMax);
    }


    private void OnZoomOut(InputAction.CallbackContext context)
    {
        zoomTarget += 1.0f;
        zoomTarget = Mathf.Clamp(zoomTarget, zoomMin, zoomMax);
    }
}

