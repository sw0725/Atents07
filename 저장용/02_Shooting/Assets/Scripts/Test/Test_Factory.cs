using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Test_Factory : TestBase
{
    public PoolObjectType PoolObjectType;
    public Vector3 Position = Vector3.zero;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetObject(PoolObjectType, Position);
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        switch (PoolObjectType)
        {
            case PoolObjectType.PlayerBullet:
                Factory.Instance.GetBullet(Position);
                break;
            case PoolObjectType.HitEffect:
                Factory.Instance.GetHitEffect(Position);
                break;
            case PoolObjectType.Expolsive:
                Factory.Instance.Getexpolsion(Position);
                break;
            case PoolObjectType.EnemyWave:
                Factory.Instance.GetEnemyWave(Position);
                break;
        }
    }
}
