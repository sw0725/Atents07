using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllerCell : Cell
{
    public int setGroup;
    const int NotSet = -1;
    public EllerCell(int x, int y) : base(x, y)
    {
        setGroup = NotSet;
    }
}
public class Eller : Maze
{
    protected override void OnSpecificAlgorithmExcute()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells[GridToIndex(x, y)] = new BackTrackingCell(x, y);      //셀 생성
            }
        }
        FirstLine();
        LineAssamble(0);
    }

    void FirstLine() 
    {
        for (int x = 0; x < width; x++)
        {
            EllerCell cell = (EllerCell)cells[GridToIndex(x, 0)];
            cell.setGroup = x;
        }
    }

    void LineAssamble(int y)
    {
        EllerCell[] cels = new EllerCell[width];
        for (int x = 0; x < width; x++)
        {
            cels[x] = (EllerCell)cells[GridToIndex(x, y)];
        }
        for (int x = 0; x < width-1; x++)
        {
            if (cels[x].setGroup != cels[x + 1].setGroup) 
            {
                if (Random.Range(0, 1) > 0.5f) 
                {
                    //벽제거
                    cels[x + 1].setGroup = cels[x].setGroup;
                }
            }
        }
    }
}
///한줄 만들기
///위 줄을 참조해서한줄 만들기(위셀에 벽없으면 위쪽셀과 같은 집합에 포함,벽 있으면 새 집합
///첫줄은 가로길이만큼 셀 작성, 각각 고유집합에 포함
///옆칸끼리 합치기
///서로 집합이 다르면 랜덤하게 벽제거, 같은 집합으로, 같은 집합일시 패스(같은줄의 같은 종류의 셀이 한번에 바뀜
///아래벽 제거하기
///같은 집합당 최소 하나이상의 벽이 제거됨
///한줄완료->1번
///마지막 줄 정리
///생성까진 동일 합칠때 세트가 다르면 무조건 합침
