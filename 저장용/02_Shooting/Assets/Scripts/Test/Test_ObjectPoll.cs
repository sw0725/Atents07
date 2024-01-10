using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ObjectPoll : TestBase
{
    public BulletPool pool;
    public EnemyPool enemyPool;
    public Expolsion HitPool;
    public Expolsion ExPool;

    private void Start()
    {
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Bullet bullet = pool.GetObject();
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Expiosion Hit = HitPool.GetObject();
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
       Enemy enemy = enemyPool.GetObject();
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Expiosion expiosion = ExPool.GetObject();
    }
}
