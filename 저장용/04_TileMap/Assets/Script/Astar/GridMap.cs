using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    protected Node[] nodes;           //2���� �迭�� �������� ���� �ʴ°� ����

    protected int width;
    protected int height;

    protected GridMap() { }

    public GridMap(int width, int height) 
    {
        this.width = width;
        this.height = height;

        nodes = new Node[width * height];

        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++) 
            {
                if(GridToIndex(x, y, out int? index))
                    nodes[index.Value] = new Node(x, y);      
            }
        }
    }

    public void ClearMapData() 
    {
        foreach (Node node in nodes) 
        {
            node.ClearData();           //�ٸ� ��ã�⶧ �������� �ʱ�ȭ
        }
    }

    public Node GetNode(int x, int y)   //Ư�� ��ǥ�� ��� ����
    {
        Node node = null;
        if (GridToIndex(x, y, out int? index)) 
        {
            node = nodes[index.Value];
        }
        return node;
    }

    public Node GetNode(Vector2Int position) 
    {
        return GetNode(position.x, position.y);
    }

    public bool IsWall(int x, int y) 
    {
        Node node = GetNode(x, y);
        return node != null && node.type == Node.NodeType.Wall;
    }

    public bool IsWall(Vector2Int position)
    {
        return IsWall(position.x, position.y);
    }

    public bool IsSlime(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.type == Node.NodeType.Slime;
    }

    public bool IsSlime(Vector2Int position)
    {
        return IsSlime(position.x, position.y);
    }

    public bool IsPlain(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.type == Node.NodeType.Plain;
    }

    public bool IsPlain(Vector2Int position)
    {
        return IsPlain(position.x, position.y);
    }

    protected bool GridToIndex(int x, int y, out int? index)  //��ǥ�� �ε���������
    {
        bool result = false;
        index = null;

        if (isValidPosition(x, y)) 
        {
            index = CalcIndex(x, y);
            result = true;
        }

        return result;
    }

    protected virtual int CalcIndex(int x, int y) 
    {
        return x + y * width;           //2���� �迭�� ���ٷ� Ǯ�������
    }

    public Vector2Int IndexToGrid(int index) 
    {
        return new Vector2Int(index % width, index / width);
    }

    public virtual bool isValidPosition(int x, int y) //GridToIndex �� �鰣 ��ǥ�� ��ȿ�� ��ǥ�ΰ� �ĺ�
    {
        return  x < width && y < height && x >= 0 && y >= 0 ;
    }

    public bool isValidPosition(Vector2Int position) //GridToIndex �� �鰣 ��ǥ�� ��ȿ�� ��ǥ�ΰ� �ĺ�
    {
        return isValidPosition(position.x, position.y);
    }
}
