using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;

    public BackTrackingCell(int x, int y) : base(x, y) 
    {
        visited = false;
    }
}

public class BackTracking : Maze
{
    protected override void OnSpecificAlgorithmExcute()         //재귀적 백트래킹 알고리즘(Recurcive BackTracking)
    {

    }

    void MakeRecursive(int x, int y) 
    {
    
    }

    //미로에서 랜덤한 지점을 미로에 임시로 추가(시작점)
    //마지막에 미로에 추가한 지점에서 갈수 있는방향중 하나를 선택해서 랜덤하게 이동한다.
    //이동한곳은 미로에 임시 추가되고 이전 지점과의 통로가 연결된다.
    //이동할 곳이 없을 경우 이전단계의 셀로 돌아간다.
    //시작지점까지돌아가면 알고리즘 종료
}
