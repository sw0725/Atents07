using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Test_NavMesh : MonoBehaviour
{
    NavMeshAgent agent;
    TestInputAction action;

    private void Awake()
    {
        action = new TestInputAction();
        agent = GetComponent<NavMeshAgent>();

        //agent.remainingDistance //���������� �����Ÿ�
        //agent.pathPending //��� �����
        //agent.autoRepath //��� ���� ����
        //agent.steeringTarget //Ÿ�ٹٶ󺸱�
    }

    private void OnEnable()
    {
        action.Test.Enable();
        action.Test.LClick.performed += OnLClick;
        action.Test.RClick.performed += OnRClick;
    }

    private void OnDisable()
    {
        action.Test.LClick.performed -= OnLClick;
        action.Test.RClick.performed -= OnRClick;
        action.Test.Disable();
    }

    private void OnLClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screen);
        if (Physics.Raycast(ray, out RaycastHit hitInfo)) 
        {
            agent.SetDestination(hitInfo.point);
        }
    }
    private void OnRClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screen);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            agent.Warp(hitInfo.point);
        }
    }
}
