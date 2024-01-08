using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Delegate : TestBase
{
    public delegate void TestDelegate();

    TestDelegate aaa;

    void TestRun() 
    {
        Debug.Log("D1");
    }
    void TestRun2()
    {
        Debug.Log("D2");
    }
    void TestRun3()
    {
        Debug.Log("D3");
    }

    private void Start()
    {
        aaa += TestRun;
        aaa += TestRun2;
        aaa += TestRun3;
    }

    protected void OnTest2() 
    {
        aaa();
    }
}
