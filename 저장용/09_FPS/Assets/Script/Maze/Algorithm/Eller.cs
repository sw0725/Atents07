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
                cells[GridToIndex(x, y)] = new BackTrackingCell(x, y);      //�� ����
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
                    //������
                    cels[x + 1].setGroup = cels[x].setGroup;
                }
            }
        }
    }
}
///���� �����
///�� ���� �����ؼ����� �����(������ �������� ���ʼ��� ���� ���տ� ����,�� ������ �� ����
///ù���� ���α��̸�ŭ �� �ۼ�, ���� �������տ� ����
///��ĭ���� ��ġ��
///���� ������ �ٸ��� �����ϰ� ������, ���� ��������, ���� �����Ͻ� �н�(�������� ���� ������ ���� �ѹ��� �ٲ�
///�Ʒ��� �����ϱ�
///���� ���մ� �ּ� �ϳ��̻��� ���� ���ŵ�
///���ٿϷ�->1��
///������ �� ����
///�������� ���� ��ĥ�� ��Ʈ�� �ٸ��� ������ ��ħ
