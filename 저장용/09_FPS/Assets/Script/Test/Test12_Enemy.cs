using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test12_Enemy : TestBase
{
    public Enemy enemy;
    public Transform respawn;

    private void Start()
    {
        enemy.Respawn(respawn.position);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        base.OnTest1(context);
    }
}

//적의 상태 구현, 기즈모 그리기
