using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test12_Enemy : TestBase
{
    public Enemy enemy;
    public Transform respawn;

    private void Start()
    {
        enemy.Respawn(respawn.position);
    }
}

//적의 상태 구현, 기즈모 그리기
