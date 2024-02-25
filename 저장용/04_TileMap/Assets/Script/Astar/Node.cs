using System;
using System.Collections;
using System.Collections.Generic;       //����Ÿ�԰��� ��
using UnityEngine;          //node1 == node2 �񱳴� �޸𸮺񱳸� ���� ������ ����
            //���� ��ĭ      //node1 > node2 �����̳� list.Sort() ��������
public class Node   : IComparable<Node> //���� �񱳰����� Ŭ������ ǥ��
{
    private int x;           //�׸���ʿ����� ��ǥ
    private int y;

    public int X => x;
    public int Y => y;

    public float G;         //��������� �� �������� �����Ÿ�
    public float H;         //�� ��忡�� ������������ �����Ÿ�

    public float F => G + H;        //���� ���� ��尡 ���õȴ�

    public enum NodeType 
    {
        Plain,
        Wall,
        Slime
    }
    public NodeType type = NodeType.Plain;

    public Node parent;     //�ٷ� ������ ���Ŀ� ���

    public Node(int x, int y, NodeType nodeType = NodeType.Plain) 
    {
        this.x = x;
        this.y = y;                 //�ٲ��� �幮�͵�
        this.type = nodeType;   
        ClearData();                //�ٲ��� �����͵�
    }

    public void ClearData()         //��ã�⸦ �Ҷ����� �ʱ�ȭ �ϱ����� �Լ�
    {           //�� ������ Ÿ���� ���� ū ��
        G = float.MaxValue;       //���Ž� �������� �۾ƾ� ��ü�Ǳ⿡ 0���� �ʱ�ȭx
        H = float.MaxValue;

        parent = null;
    }

    public int CompareTo(Node other)        //���� Ÿ�԰� ũ���
    {                       //return���:: 1 ����ũ��, 0 ����, -1 �����۴�
        if(other == null)   //����ִ°Ͱ� ��, ����� �۴�
            return 1;

        return F.CompareTo(other.F);        //F���� �������� ������ ���Ѵ�.
    }
                       //***������ �������̵�***
    //public static bool operator ==(Node left, Node right) 
    //{                //node1 == node2�� ��ġ���ϱ����� 
    //    return (left.x == right.x && left.y == right.y);
    //}                //CompareTo�� ohter == null ������� �浹
                       //�׳� �� ��ǥ�� �����ϱ�� ��
    public static bool operator ==(Node left, Vector2Int right)
    {
        return (left.x == right.x && left.y == right.y);
    }

                       //*** == �������̵��� !=�� �������̵� �ؾ���***
    //public static bool operator !=(Node left, Node right)
    //{
    //    return (left.x != right.x || left.y != right.y);
    //}

    public static bool operator !=(Node left, Vector2Int right)
    {
        return (left.x != right.x || left.y != right.y);
    }

    public override bool Equals(object obj)
    {
        return obj is Node other && this.x == other.x && this.y == other.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);  //��ġ�� 2���� �ؽ��ڵ� �����
    }
}
