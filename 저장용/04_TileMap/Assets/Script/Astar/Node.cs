using System;
using System.Collections;
using System.Collections.Generic;       //참조타입간의 비교
using UnityEngine;          //node1 == node2 비교는 메모리비교를 뜻함 같을수 없다
            //맵의 한칸      //node1 > node2 연산이나 list.Sort() 정렬위함
public class Node   : IComparable<Node> //노드는 비교가능한 클래스다 표시
{
    private int x;           //그리드맵에서의 좌표
    private int y;

    public int X => x;
    public int Y => y;

    public float G;         //출발점에서 이 노드까지의 실제거리
    public float H;         //이 노드에서 목적지까지의 직선거리

    public float F => G + H;        //가장 작은 노드가 선택된다

    public enum NodeType 
    {
        Plain,
        Wall,
        Slime
    }
    public NodeType type = NodeType.Plain;

    public Node parent;     //바로 직전에 거쳐온 노드

    public Node(int x, int y, NodeType nodeType = NodeType.Plain) 
    {
        this.x = x;
        this.y = y;                 //바뀔일 드문것들
        this.type = nodeType;   
        ClearData();                //바뀔일 많은것들
    }

    public void ClearData()         //길찾기를 할때마다 초기화 하기위한 함수
    {           //현 데이터 타입의 가장 큰 값
        G = float.MaxValue;       //갱신시 기존보다 작아야 교체되기에 0으로 초기화x
        H = float.MaxValue;

        parent = null;
    }

    public int CompareTo(Node other)        //같은 타입간 크기비교
    {                       //return경우:: 1 내가크다, 0 같다, -1 내가작다
        if(other == null)   //비어있는것과 비교, 빈것이 작다
            return 1;

        return F.CompareTo(other.F);        //F값을 기준으로 순서를 정한다.
    }
                       //***연산자 오버라이딩***
    //public static bool operator ==(Node left, Node right) 
    //{                //node1 == node2로 위치비교하기위한 
    //    return (left.x == right.x && left.y == right.y);
    //}                //CompareTo의 ohter == null 연산과의 충돌
                       //그냥 깡 좌표로 연산하기로 함
    public static bool operator ==(Node left, Vector2Int right)
    {
        return (left.x == right.x && left.y == right.y);
    }

                       //*** == 오버라이딩시 !=도 오버라이딩 해야함***
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
        return HashCode.Combine(x, y);  //위치값 2개로 해쉬코드 만들기
    }
}
