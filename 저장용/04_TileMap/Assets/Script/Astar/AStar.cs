using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    const float sideDistance = 1.0f;
    const float diagonalDistance = 1.4f;

    public static List<Vector2Int> PathFind(GridMap map, Vector2Int start, Vector2Int end) 
    {
        List<Vector2Int> path = null;

        if (map.isValidPosition(start) && map.isValidPosition(end) && map.IsPlain(start) && map.IsPlain(end)) 
        {
            map.ClearMapData();

            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();

            Node current = map.GetNode(start);
            current.G = 0.0f;
            current.H = GetHeuristic(current, end);
            open.Add(current);

            while (open.Count > 0) //���¸���Ʈ�� ���� ��������� ��ӵ��� -> ���� ���� = ���� ����
            {
                open.Sort();        // f�� �������� ����
                current = open[0];
                open.RemoveAt(0);   //��� ���� - Close��

                if (current != end)
                {
                    close.Add(current);
                    for (int y = -1; y < 2; y++) //���� 8���� Ž��
                    {
                        for (int x = -1; x < 2; x++) 
                        {
                            Node node = map.GetNode(current.X + x, current.Y + y);
                                // ��ŵ�� ����ΰ�? ������ ����(�������� ���������� ���� ���Ҵٸ� ����Ų��.)
                            if ( node == null ) continue;    // �� ��
                            if ( node == current) continue;  //�ڱ��ڽ�
                            if ( node.type == Node.NodeType.Wall ) continue;  //��
                            if (close.Exists((x) => x == node)) continue;     //close����Ʈ
                            
                            bool isDiagonal = (x * y) != 0;     //�밢���ΰ�
                            if (isDiagonal &&
                                (map.IsWall(current.X + x, current.Y) || map.IsWall(current.X, current.Y + y)))  //��濡 �����ִ°�
                                continue;                    //���� �����ִ� �밢��
                                //current���� xy���� ���µ� �ɸ��� �Ÿ� Ȯ��
                            float distanse = isDiagonal ? diagonalDistance : sideDistance;

                            if (node.G > current.G + distanse) //���� ���� g���� ����
                            {
                                if (node.parent == null) //open�� �ִ°�
                                {                        //����Ʈ�� �������� �ݵ�� �θ� ����������
                                    node.H = GetHeuristic(node, end);
                                    open.Add(current);
                                }
                                node.G = current.G + distanse;
                                node.parent = current;
                            }
                        }
                    }
                }
                else                //������ ����
                {
                    break;
                }
            }

            if (current == end)     //�� ã�� => ����ۼ� 
            {
                path = new List<Vector2Int>();
                Node result = current;
                while (result != null) //�θ� ���� ��� = ������
                {
                    path.Add(new Vector2Int(result.X, result.Y));
                    result = result.parent;
                }
                path.Reverse(); // ������ -> ���������� �Ǿ��ִ� ��� ������
            }
        }
        return path;
    }
                        //����ġ���� ���������� �����Ÿ� ���
    private static float GetHeuristic(Node current, Vector2Int end) 
    {
        return Mathf.Abs(current.X - end.x) + Mathf.Abs(current.Y - end.y);
    }               //���밪��ȯ
}
