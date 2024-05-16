using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Board Board => board;
    protected Board board;
    public Ship[] Ships => ships;
    protected Ship[] ships;

    public Action onActionEnd;
    public Action<bool> onAttackFail;
    public Action onDefeat;
    int remainShipCount;
    bool isActionDone = false;

    public int SuccessAttackCount => successAttackCount;
    int successAttackCount;
    public int FailAttackCount => failAttackCount;
    int failAttackCount;
    public int TotalAttackCount => failAttackCount + successAttackCount;

    public GameObject criticalMarkPrefab;
    Dictionary<uint, GameObject> criticalMark;
    Transform criticalMarkParent;

    List<uint> normalAttackIndex;
    List<uint> criticalAttackIndex;     //우선순위 높은 공격지역

    Vector2Int lastSuccessAttackPos;    //가장 최근 성공한 공격지역, 값이Not_Success이면 최근의 공격이 실패했음을 뜻함

    bool opponentShipDestroyed = false;

    protected PlayerBase opponent;

    protected ShipManager shipManager;
    protected GameManager gameManager;
    protected TurnControler turnManager;

    readonly Vector2Int Not_Success = -Vector2Int.one;
    readonly Vector2Int[] neighbors = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };

    protected virtual void Awake()
    {
        board = GetComponentInChildren<Board>();
        criticalMarkParent = transform.GetChild(1);
        criticalMark = new Dictionary<uint, GameObject>(10);
    }

    protected virtual void Start()
    {
        shipManager = ShipManager.Instance;
        gameManager = GameManager.Instance;
        turnManager = gameManager.TurnManager;
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
        remainShipCount = count;


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

        turnManager.onTurnStart += OnPlayerTurnStart;
        turnManager.onTurnEnd += OnPlayerTurnEnd;

        successAttackCount = 0;
        failAttackCount = 0;
    }

    //함선 배치 =====================================

    public void AutoShipDeployment(bool isShipShow) //isShipShips = 베치후함선표시?
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
                ship.gameObject.SetActive(isShipShow);

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
        if(!isActionDone && opponentBoard.IsInBoard(attackGrid) && opponentBoard.IsAttackable(attackGrid)) 
        {
            bool result = opponentBoard.OnAttacked(attackGrid);
            if (result) 
            {
                successAttackCount++;
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
                failAttackCount++;
                onAttackFail?.Invoke(this is UserPlayer);
            }

            uint attackIndex = (uint)board.GridToIndex(attackGrid).Value;
            RemoveCriticalPos(attackIndex);
            normalAttackIndex.Remove(attackIndex);

            isActionDone = true;
            onActionEnd?.Invoke();
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

    void AddCriticalFromTwoPoint(Vector2Int now, Vector2Int last) 
    {
        if (IsSuccessLine(last, now, true)) //양끝을 크리추가
        {
            Vector2Int grid;
            List<uint> deleteTarget = new List<uint>(16);
            foreach(var index in criticalAttackIndex)           //같은 줄(y)이 아니라면 제거
            {
                grid = Board.IndexToGrid(index);
                if(grid.y != now.y) 
                {
                    deleteTarget.Add(index);
                }
            }
            foreach (var index in deleteTarget)
            {
                RemoveCriticalPos(index);
            }

            grid = now;
            for (grid.x = now.x + 1; grid.x < Board.BoardSize; grid.x++) 
            {
                if (!Board.IsInBoard(grid)) break;
                if (opponent.Board.IsAttackFailPosition(grid)) break;
                if (opponent.Board.IsAttackable(grid)) 
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);
                    break;
                }
            }
            for(grid.x = now.x - 1; grid.x > -1; grid.x--) 
            {
                if (!Board.IsInBoard(grid)) break;
                if (opponent.Board.IsAttackFailPosition(grid)) break;
                if (opponent.Board.IsAttackable(grid))
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);
                    break;
                }
            }
        }
        else if (IsSuccessLine(last, now, false))
        {
            Vector2Int grid;
            List<uint> deleteTarget = new List<uint>(16);
            foreach (var index in criticalAttackIndex)           //같은 줄(x)이 아니라면 제거
            {
                grid = Board.IndexToGrid(index);
                if (grid.x != now.x)
                {
                    deleteTarget.Add(index);
                }
            }
            foreach (var index in deleteTarget)
            {
                RemoveCriticalPos(index);
            }

            grid = now;
            for (grid.y = now.y + 1; grid.y < Board.BoardSize; grid.y++)
            {
                if (!Board.IsInBoard(grid)) break;
                if (opponent.Board.IsAttackFailPosition(grid)) break;
                if (opponent.Board.IsAttackable(grid))
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);
                    break;
                }
            }
            for (grid.y = now.y - 1; grid.y > -1; grid.y--)
            {
                if (!Board.IsInBoard(grid)) break;
                if (opponent.Board.IsAttackFailPosition(grid)) break;
                if (opponent.Board.IsAttackable(grid))
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);
                    break;
                }
            }
        }
        else                                //같은줄이 아님 = 다른배
        {
            AddCriticalFromNeighbors(now);
        }
    }

    bool IsSuccessLine(Vector2Int start, Vector2Int end, bool isHorizontal) 
    {
        bool result = true;

        Vector2Int pos = start;
        int dir = 1;
        if (isHorizontal) 
        {
            if(start.y == end.y)                //float 은 == 로 비교하지 마라
            {
                if(start.x > end.x) 
                {
                    dir = -1;
                }

                start.x *= dir;                 //역방향일 경우 방향 뒤집기
                end.x *= dir;
                end.x++;                        //end의 x까지 확인 위함

                for(int i = start.x; i < end.x; i++) 
                {
                    pos.x = i * dir;            //뒤집은것 다시 뒤집기
                    if (opponent.Board.IsAttackFailPosition(pos)) 
                    {
                        result = false;
                        break;
                    }
                }
            }
            else 
            {
                result = false;
            }
        }
        else 
        {
            if (start.x == end.x)                //float 은 == 로 비교하지 마라
            {
                if (start.y > end.y)
                {
                    dir = -1;
                }

                start.y *= dir;                 //역방향일 경우 방향 뒤집기
                end.y *= dir;
                end.y++;                        //end의 y까지 확인 위함

                for (int i = start.y; i < end.y; i++)
                {
                    pos.y = i * dir;            //뒤집은것 다시 뒤집기
                    if (opponent.Board.IsAttackFailPosition(pos))
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }
        }
        return result;
    }

    void AddCriticalFromNeighbors(Vector2Int grid)
    {
        Util.Shuffle(neighbors);
        foreach(var neighbor in neighbors) 
        {
            Vector2Int pos = grid + neighbor;
            if (opponent.Board.IsAttackable(pos)) 
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

            if (GameManager.Instance.IsTestMode)
            {
                GameObject obj = Instantiate(criticalMarkPrefab, criticalMarkParent);
                obj.transform.position = opponent.Board.IndexToWorld(index);
                Vector2Int grid = opponent.Board.IndexToGrid(index);
                obj.name = $"Critical_({grid.x}, {grid.y})";
                criticalMark[index] = obj;
            }
        }
    }

    void RemoveAllCriticalPos() 
    {
        while(criticalMarkParent.childCount > 0) 
        {
            Transform c = criticalMarkParent.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
        criticalMark.Clear();                       //딕셔너리 초기화

        criticalAttackIndex.Clear();
        lastSuccessAttackPos = Not_Success;
    }
    void RemoveCriticalPos(uint index)
    {
        if (criticalAttackIndex.Contains(index)) 
        {
            criticalAttackIndex.Remove(index);

        }
        if (criticalMark.ContainsKey(index))    //없는키값에 액세스시 터짐
        {
            Destroy(criticalMark[index]);       //게임오브제 지우고
            criticalMark[index] = null;         //값 초기화
            criticalMark.Remove(index);         //키값 제거
        }
    }

    //턴 관리용 =====================================

    protected virtual void OnPlayerTurnStart(int _) 
    {
        isActionDone = false;
    }

    protected virtual void OnPlayerTurnEnd()
    {
        if(!isActionDone) 
        {
            AutoAttack();
        }
    }

    //침몰 패배 =====================================

    void OnShipDestroy(Ship ship) 
    {
        opponent.opponentShipDestroyed = true;          //내 함선이 침몰시 상대에게 알림
        opponent.lastSuccessAttackPos = Not_Success;

        remainShipCount--;
        if(remainShipCount < 1) 
        {
            OnDefeat();
        }
    }

    protected virtual void OnDefeat() 
    {
        Debug.Log($"{gameObject.name}패배");
        onDefeat?.Invoke(); 
    }

    //기타 =========================================
    public void Clear() //플레이어 초기화용
    {
        opponentShipDestroyed = false;
        Board.ResetBoard(Ships);
    }

    public Ship GetShip(ShipType shipType) 
    {
        return (shipType != ShipType.None) ? Ships[(int)shipType - 1] : null;
    }

    //테스트 =========================================

#if UNITY_EDITOR
    public void Test_IsSuccessLine(Vector2Int grid) 
    {
        if(IsSuccessLine(grid, lastSuccessAttackPos, true)) 
        {
            Debug.Log("수평공격 성공");
        }
        else if (IsSuccessLine(grid, lastSuccessAttackPos, false))
        {
            Debug.Log("수직공격 성공");
        }
        else 
        {
            Debug.Log("한 줄 아님");
        }
    }
#endif
}
