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

            while (open.Count > 0) //오픈리스트에 뭐라도 들어있으면 계속돈다 -> 뭐가 없다 = 길이 없다
            {
                open.Sort();        // f값 기준으로 정렬
                current = open[0];
                open.RemoveAt(0);   //노드 선택 - Close로

                if (current != end)
                {
                    close.Add(current);
                    for (int y = -1; y < 2; y++) //주위 8방향 탐색
                    {
                        for (int x = -1; x < 2; x++) 
                        {
                            Node node = map.GetNode(current.X + x, current.Y + y);
                                // 스킵할 노드인가? 슬라임 제외(슬라임은 움직임으로 길을 막았다면 대기시킨다.)
                            if ( node == null ) continue;    // 맵 밖
                            if ( node == current) continue;  //자기자신
                            if ( node.type == Node.NodeType.Wall ) continue;  //벽
                            if (close.Exists((x) => x == node)) continue;     //close리스트
                            
                            bool isDiagonal = (x * y) != 0;     //대각선인가
                            if (isDiagonal &&
                                (map.IsWall(current.X + x, current.Y) || map.IsWall(current.X, current.Y + y)))  //사방에 벽이있는가
                                continue;                    //옆에 벽이있는 대각선
                                //current에서 xy까지 가는데 걸리는 거리 확정
                            float distanse = isDiagonal ? diagonalDistance : sideDistance;

                            if (node.G > current.G + distanse) //원래 보다 g값이 작음
                            {
                                if (node.parent == null) //open에 있는가
                                {                        //리스트에 넣을때는 반드시 부모가 설정됨으로
                                    node.H = GetHeuristic(node, end);
                                    open.Add(current);
                                }
                                node.G = current.G + distanse;
                                node.parent = current;
                            }
                        }
                    }
                }
                else                //목적지 도달
                {
                    break;
                }
            }

            if (current == end)     //길 찾음 => 경로작성 
            {
                path = new List<Vector2Int>();
                Node result = current;
                while (result != null) //부모가 없는 노드 = 시작점
                {
                    path.Add(new Vector2Int(result.X, result.Y));
                    result = result.parent;
                }
                path.Reverse(); // 도착점 -> 시작점으로 되어있는 경로 뒤집기
            }
        }
        return path;
    }
                        //현위치에서 목적지까지 직선거리 계산
    private static float GetHeuristic(Node current, Vector2Int end) 
    {
        return Mathf.Abs(current.X - end.x) + Mathf.Abs(current.Y - end.y);
    }               //절대값반환
}
