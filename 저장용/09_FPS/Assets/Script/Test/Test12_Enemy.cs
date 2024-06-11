using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test12_Enemy : TestBase
{
    public Enemy enemy;
    public Transform respawn;

    public Enemy.BehaviorState behaviorState = Enemy.BehaviorState.Wander;

    private void Start()
    {
        enemy.Respawn(respawn.position);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        enemy.Test_StateChange(behaviorState);
    }
}

//¹èÈ¸ ³ì»ö, ÃßÀû ÁÖÈ², Å½»ö ÆÄ¶û, °ø°Ý »¡°­, »ç¸Á °ËÁ¤
