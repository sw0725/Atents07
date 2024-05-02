using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Board Board => board;
    protected Board board;
    public Ship[] Ships => ships;
    protected Ship[] ships;

    protected PlayerBase opponent;

    protected ShipManager shipManager;

    protected GameManager gameManager;

    List<uint> normalAttackIndex;
    List<uint> criticalAttackIndex;     //우선순위 높은 공격지역

    Vector2Int lastSuccessAttackPos;    //가장 최근 성공한 공격지역, 값이Not_Success이면 최근의 공격이 실패했음을 뜻함

    bool opponentShipDestroyed = false;

    readonly Vector2Int Not_Success = -Vector2Int.one;
    readonly Vector2Int[] neighbors = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };

    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    protected virtual void Start()
    {
        shipManager = ShipManager.Instance;
        gameManager = GameManager.Instance;
        Initialize();
    }

    protected void Initialize()
    {
        int count = shipManager.ShipTypeCount;
        ships = new Ship[count];
        for(int i = 0; i < count; i++) 
        {
            ShipType shipType = (ShipType)i + 1;
            ships[i] = shipManager.MakeShip(shipType, transform);

            ships[i].onHit += (_) => gameManager.CameraShake(1);
            ships[i].onSink += (_) => gameManager.CameraShake(3);
            ships[i].onSink += OnShipDestroy;

            board.onShipAttacked[shipType] += ships[i].OnHitted;
        }
        
        Board.ResetBoard(ships);

        int fullSize = Board.BoardSize * Board.BoardSize;
        uint[] temp = new uint[fullSize];
        for (uint i = 0; i < fullSize; i++)
        {
            temp[i] = i;
        }
        Util.Shuffle(temp);
        normalAttackIndex = new List<uint>(temp);       //랜덤으로 섞은 공격 후보지역

        criticalAttackIndex = new List<uint>(10);
        lastSuccessAttackPos = Not_Success;
    }

    //함선 배치 =====================================

    public void AutoShipDeployment(bool isShipShips) //isShipShips = 베치후함선표시?
    {
        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> high = new List<int>(maxCapacity);
        List<int> low = new List<int>(maxCapacity);

        for (int i = 0; i < maxCapacity; i++)
        {
            if ((i % Board.BoardSize == 0) || (i % Board.BoardSize == (Board.BoardSize - 1)) || (i > 0 && i < Board.BoardSize - 1) || (Board.BoardSize * (Board.BoardSize - 1) < i && i < Board.BoardSize * Board.BoardSize - 1)) //보드의 좌표를인덱스로 변환시 분류기준으로 (0,10,20...) = (x = 10 y = i)||(9,19,29...)||(1,2,3...)||(91,92,93...)
            {
                low.Add(i); //테두리
            }
            else
            {
                high.Add(i);
            }
        }

        foreach (var ship in Ships)
        {
            if (ship.IsDeployed)
            {
                int[] shipIndice = new int[ship.Size];
                for (int i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = board.GridToIndex(ship.Positions[i]).Value;
                }
                foreach (var index in shipIndice)                               //배가 배치된곳 빼기
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPos(ship);                       //배치 주변부 그하기
                foreach (var index in toLow)
                {
                    high.Remove(index);
                    low.Add(index);
                }
            }
        }

        int[] temp = high.ToArray();
        Util.Shuffle(temp);
        high = new(temp);

        temp = low.ToArray();
        Util.Shuffle(temp);
        low = new(temp);

        foreach (var ship in Ships)
        {
            if (!ship.IsDeployed)
            {
                ship.RandomRotate();

                bool fail = true;
                int count = 0;
                Vector2Int grid;
                Vector2Int[] shipPos;
                do
                {
                    int head = high[0];
                    high.RemoveAt(0);

                    grid = board.IndexToGrid((uint)head);
                    fail = !board.IsShipDeploymentAvailable(ship, grid, out shipPos);
                    if (fail)
                    {
                        high.Add(head);
                    }
                    else
                    {
                        for (int i = 1; i < shipPos.Length; i++)
                        {
                            int body = board.GridToIndex(shipPos[i]).Value;
                            if (!high.Contains(body))
                            {
                                high.Add(head);
                                fail = true;
                                break;
                            }
                        }
                    }
                    count++;
                } while (fail && count < 10 && high.Count > 0);

                count = 0;
                while (fail && count < 1000)
                {
                    int head = low[0];
                    low.RemoveAt(0);
                    grid = board.IndexToGrid((uint)head);
                    fail = !board.IsShipDeploymentAvailable(ship, grid, out shipPos);
                    if (fail)
                    {
                        low.Add(head);
                    }
                    count++;
                }

                if (fail)
                {
                    Debug.LogWarning("자동배치 실패");
                    return;
                }

                board.ShipDeployment(ship, grid);   //실배치
                ship.gameObject.SetActive(isShipShips);

                List<int> tempList = new List<int>(shipPos.Length);
                foreach (var pos in shipPos)
                {
                    tempList.Add(board.GridToIndex(pos).Value);
                }
                foreach (var index in tempList)
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPos(ship);                       //배치 주변부 그하기
                foreach (var index in toLow)
                {
                    if (high.Contains(index))
                    {
                        high.Remove(index);
                        low.Add(index);
                    }
                }
            }
        }
    }

    List<int> GetShipAroundPos(Ship ship)
    {
        List<int> result = new List<int>(ship.Size * 2 + 6);
        int? index = null;
        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            foreach (var pos in ship.Positions)
            {
                index = board.GridToIndex(pos + Vector2Int.left);
                if (index.HasValue) result.Add(index.Value);

                index = board.GridToIndex(pos + Vector2Int.right);
                if (index.HasValue) result.Add(index.Value);
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.North)
            {
                head = ship.Positions[0] + Vector2Int.down;
                tail = ship.Positions[^1] + Vector2Int.up;      //^1 = 뒤에서 1번째(이경우 1부터 시작 == 마지막 요소)
            }
            else
            {

                head = ship.Positions[0] + Vector2Int.up;
                tail = ship.Positions[^1] + Vector2Int.down;
            }

            index = board.GridToIndex(head);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.left);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.right);
            if (index.HasValue) result.Add(index.Value);

            index = board.GridToIndex(tail);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.left);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.right);
            if (index.HasValue) result.Add(index.Value);
        }
        else
        {
            foreach (var pos in ship.Positions)
            {
                index = board.GridToIndex(pos + Vector2Int.up);
                if (index.HasValue) result.Add(index.Value);

                index = board.GridToIndex(pos + Vector2Int.down);
                if (index.HasValue) result.Add(index.Value);
            }

            Vector2Int head;
            Vector2Int tail;
            if (ship.Direction == ShipDirection.East)
            {
                head = ship.Positions[0] + Vector2Int.right;
                tail = ship.Positions[^1] + Vector2Int.left;
            }
            else
            {

                head = ship.Positions[0] + Vector2Int.left;
                tail = ship.Positions[^1] + Vector2Int.right;
            }

            index = board.GridToIndex(head);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.up);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.down);
            if (index.HasValue) result.Add(index.Value);

            index = board.GridToIndex(tail);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.up);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.down);
            if (index.HasValue) result.Add(index.Value);
        }

        return result;
    }

    public void UndoAllShipDeployment() //모든함선 배치 취소
    {
        Board.ResetBoard(Ships);
    }

    //공격 관련 =====================================

    public void Attack(Vector2Int attackGrid) 
    {
        Board opponentBoard = opponent.Board;
        if(opponentBoard.IsInBoard(attackGrid) && opponentBoard.IsAttackable(attackGrid)) 
        {
            bool result = opponentBoard.OnAttacked(attackGrid);
            if (result) 
            {
                if (opponentShipDestroyed) //침몰
                {
                    RemoveAllCriticalPos();
                    opponentShipDestroyed =false;
                }
                else 
                {
                    if (lastSuccessAttackPos != Not_Success) //연속공격 = 한줄공격
                    {
                        AddCriticalFromTwoPoint(attackGrid, lastSuccessAttackPos);
                    }
                    else 
                    {
                        AddCriticalFromNeighbors(attackGrid);
                    }

                    lastSuccessAttackPos = attackGrid;
                }
            }
            else 
            {
                lastSuccessAttackPos = Not_Success;
            }

            uint attackIndex = (uint)board.GridToIndex(attackGrid).Value;
            RemoveCriticalPos(attackIndex);
            normalAttackIndex.Remove(attackIndex);
        }
    }

    public void Attack(Vector3 world) 
    {
        Attack(opponent.Board.WorldToGrid(world));
    }

    public void Attack(uint index)
    {
        Attack(opponent.Board.IndexToGrid(index));
    }

    public void AutoAttack() //다음목표 설정 방법 1. 무작위 공격 2. 이전공격 성공? 성공위치의 상하좌우 공격 3. 공격이 한줄로 성공?  확인순서 : 3->2->1
    {
        uint target;
        if (criticalAttackIndex.Count > 0)
        {
            target = criticalAttackIndex[0];
            criticalAttackIndex.RemoveAt(0);
            normalAttackIndex.Remove(target);       //노말의 좌표와 크리팈티컬의 좌표는 겹친다
        }
        else 
        {
            target = normalAttackIndex[0];
            normalAttackIndex.RemoveAt(0);
        }
        Attack(target);
    }

    void AddCriticalFromTwoPoint(Vector2Int grid, Vector2Int lastSuccessPos) 
    {
    }
    void AddCriticalFromNeighbors(Vector2Int grid)
    {
        Util.Shuffle(neighbors);
        foreach(var neighbor in neighbors) 
        {
            Vector2Int pos = grid + neighbor;
            if (board.IsAttackable(pos)) 
            {
                AddCritical((uint)board.GridToIndex(pos).Value);
            }
        }
    }

    void AddCritical(uint index) 
    {
        if (!criticalAttackIndex.Contains(index)) 
        {
            criticalAttackIndex.Insert(0, index);   //새로 발견된 지역이 더 가능성이 높다
        }
    }

    void RemoveAllCriticalPos() 
    {
        criticalAttackIndex.Clear();
        lastSuccessAttackPos = Not_Success;
    }
    void RemoveCriticalPos(uint index)
    {
        if (criticalAttackIndex.Contains(index)) 
        {
            criticalAttackIndex.Remove(index);
        }
    }

    //턴 관리용 =====================================
    //침몰 패배 =====================================

    void OnShipDestroy(Ship ship) 
    {
        opponent.opponentShipDestroyed = true;          //내 함선이 침몰시 상대에게 알림
        opponent.lastSuccessAttackPos = Not_Success;
    }

    //기타 =========================================
    public void Clear() //플레이어 초기화용
    {
        opponentShipDestroyed = false;
        Board.ResetBoard(Ships);
    }
    //테스트 =========================================
}
