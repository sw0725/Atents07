using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_SceneAdditiveLoad : TestBase
{
    [Range(0, 2)]
    public int targetX = 0;
    [Range(0, 2)]
    public int targetY = 0;

    WorldManager world;

    private void Start()
    {
        world = GameManager.Instance.World;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {                                           //��������� �߰������� ���� �ҷ����� �ִ�.
        SceneManager.LoadScene($"SceneLess_{targetX}{targetY}", LoadSceneMode.Additive);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {//�ش���� �����Ѵ�.-�񵿱�θ� ����
        SceneManager.UnloadSceneAsync($"SceneLess_{targetX}{targetY}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        world.TestLoadScene(targetX, targetY);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        world.TestUnloadScene(targetX, targetY);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        world.TestRefreshScene(targetX, targetY);
    }

    protected override void OnLClick(InputAction.CallbackContext context)
    {
        world.TestUnloadAllScene();
    }
}
