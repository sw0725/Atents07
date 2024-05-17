using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    PlayerInputAction actions;

    private void Awake()
    {
        actions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        actions.Title.Enable();
        actions.Title.Anyting.performed += OnAnything;
    }

    private void OnDisable()
    {
        actions.Title.Anyting.performed -= OnAnything;
        actions.Title.Disable();
    }

    private void OnAnything(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(1);
    }
}
