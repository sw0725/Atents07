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
    protected override void OnSpecificAlgorithmExcute()         //����� ��Ʈ��ŷ �˰���(Recurcive BackTracking)
    {

    }

    void MakeRecursive(int x, int y) 
    {
    
    }

    //�̷ο��� ������ ������ �̷ο� �ӽ÷� �߰�(������)
    //�������� �̷ο� �߰��� �������� ���� �ִ¹����� �ϳ��� �����ؼ� �����ϰ� �̵��Ѵ�.
    //�̵��Ѱ��� �̷ο� �ӽ� �߰��ǰ� ���� �������� ��ΰ� ����ȴ�.
    //�̵��� ���� ���� ��� �����ܰ��� ���� ���ư���.
    //���������������ư��� �˰��� ����
}
