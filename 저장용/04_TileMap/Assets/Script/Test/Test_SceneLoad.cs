using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_SceneLoad : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {       //씬파일을 불러올 동안 아무것도 못함       =>  로딩창을 보여주기 위해서라도 비동기 방식으로 로딩해야함
        SceneManager.LoadScene("LoadSampleScene");      //씬파일 이름으로 불러올수 있다
        //SceneManager.LoadScene(0);                    //씬번호로도 가능
    }                                                   //동기방식(Synchronous) 이코드가 끝나야 다른코드실행
}
