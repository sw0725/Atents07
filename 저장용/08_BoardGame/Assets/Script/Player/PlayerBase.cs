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
    List<uint> criticalAttackIndex;     //�켱���� ���� ��������

    Vector2Int lastSuccessAttackPos;    //���� �ֱ� ������ ��������, ����Not_Success�̸� �ֱ��� ������ ���������� ����

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
        normalAttackIndex = new List<uint>(temp);       //�������� ���� ���� �ĺ�����

        criticalAttackIndex = new List<uint>(10);
        lastSuccessAttackPos = Not_Success;

        turnManager.onTurnStart += OnPlayerTurnStart;
        turnManager.onTurnEnd += OnPlayerTurnEnd;

        successAttackCount = 0;
        failAttackCount = 0;
    }

    //�Լ� ��ġ =====================================

    public void AutoShipDeployment(bool isShipShow) //isShipShips = ��ġ���Լ�ǥ��?
    {
        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> high = new List<int>(maxCapacity);
        List<int> low = new List<int>(maxCapacity);

        for (int i = 0; i < maxCapacity; i++)
        {
            if ((i % Board.BoardSize == 0) || (i % Board.BoardSize == (Board.BoardSize - 1)) || (i > 0 && i < Board.BoardSize - 1) || (Board.BoardSize * (Board.BoardSize - 1) < i && i < Board.BoardSize * Board.BoardSize - 1)) //������ ��ǥ���ε����� ��ȯ�� �з��������� (0,10,20...) = (x = 10 y = i)||(9,19,29...)||(1,2,3...)||(91,92,93...)
            {
                low.Add(i); //�׵θ�
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
                foreach (var index in shipIndice)                               //�谡 ��ġ�Ȱ� ����
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPos(ship);                       //��ġ �ֺ��� ���ϱ�
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
                    Debug.LogWarning("�ڵ���ġ ����");
                    return;
                }

                board.ShipDeployment(ship, grid);   //�ǹ�ġ
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

                List<int> toLow = GetShipAroundPos(ship);                       //��ġ �ֺ��� ���ϱ�
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
                tail = ship.Positions[^1] + Vector2Int.up;      //^1 = �ڿ��� 1��°(�̰�� 1���� ���� == ������ ���)
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

    public void UndoAllShipDeployment() //����Լ� ��ġ ���
    {
        Board.ResetBoard(Ships);
    }

    //���� ���� =====================================

    public void Attack(Vector2Int attackGrid) 
    {
        Board opponentBoard = opponent.Board;
        if(!isActionDone && opponentBoard.IsInBoard(attackGrid) && opponentBoard.IsAttackable(attackGrid)) 
        {
            bool result = opponentBoard.OnAttacked(attackGrid);
            if (result) 
            {
                successAttackCount++;
                if (opponentShipDestroyed) //ħ��
                {
                    RemoveAllCriticalPos();
                    opponentShipDestroyed =false;
                }
                else 
                {
                    if (lastSuccessAttackPos != Not_Success) //���Ӱ��� = ���ٰ���
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

    public void AutoAttack() //������ǥ ���� ��� 1. ������ ���� 2. �������� ����? ������ġ�� �����¿� ���� 3. ������ ���ٷ� ����?  Ȯ�μ��� : 3->2->1
    {
        uint target;
        if (criticalAttackIndex.Count > 0)
        {
            target = criticalAttackIndex[0];
            criticalAttackIndex.RemoveAt(0);
            normalAttackIndex.Remove(target);       //�븻�� ��ǥ�� ũ���JƼ���� ��ǥ�� ��ģ��
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
        if (IsSuccessLine(last, now, true)) //�糡�� ũ���߰�
        {
            Vector2Int grid;
            List<uint> deleteTarget = new List<uint>(16);
            foreach(var index in criticalAttackIndex)           //���� ��(y)�� �ƴ϶�� ����
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
            foreach (var index in criticalAttackIndex)           //���� ��(x)�� �ƴ϶�� ����
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
        else                                //�������� �ƴ� = �ٸ���
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
            if(start.y == end.y)                //float �� == �� ������ ����
            {
                if(start.x > end.x) 
                {
                    dir = -1;
                }

                start.x *= dir;                 //�������� ��� ���� ������
                end.x *= dir;
                end.x++;                        //end�� x���� Ȯ�� ����

                for(int i = start.x; i < end.x; i++) 
                {
                    pos.x = i * dir;            //�������� �ٽ� ������
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
            if (start.x == end.x)                //float �� == �� ������ ����
            {
                if (start.y > end.y)
                {
                    dir = -1;
                }

                start.y *= dir;                 //�������� ��� ���� ������
                end.y *= dir;
                end.y++;                        //end�� y���� Ȯ�� ����

                for (int i = start.y; i < end.y; i++)
                {
                    pos.y = i * dir;            //�������� �ٽ� ������
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
            criticalAttackIndex.Insert(0, index);   //���� �߰ߵ� ������ �� ���ɼ��� ����

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
        criticalMark.Clear();                       //��ųʸ� �ʱ�ȭ

        criticalAttackIndex.Clear();
        lastSuccessAttackPos = Not_Success;
    }
    void RemoveCriticalPos(uint index)
    {
        if (criticalAttackIndex.Contains(index)) 
        {
            criticalAttackIndex.Remove(index);

        }
        if (criticalMark.ContainsKey(index))    //����Ű���� �׼����� ����
        {
            Destroy(criticalMark[index]);       //���ӿ����� �����
            criticalMark[index] = null;         //�� �ʱ�ȭ
            criticalMark.Remove(index);         //Ű�� ����
        }
    }

    //�� ������ =====================================

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

    //ħ�� �й� =====================================

    void OnShipDestroy(Ship ship) 
    {
        opponent.opponentShipDestroyed = true;          //�� �Լ��� ħ���� ��뿡�� �˸�
        opponent.lastSuccessAttackPos = Not_Success;

        remainShipCount--;
        if(remainShipCount < 1) 
        {
            OnDefeat();
        }
    }

    protected virtual void OnDefeat() 
    {
        Debug.Log($"{gameObject.name}�й�");
        onDefeat?.Invoke(); 
    }

    //��Ÿ =========================================
    public void Clear() //�÷��̾� �ʱ�ȭ��
    {
        opponentShipDestroyed = false;
        Board.ResetBoard(Ships);
    }

    public Ship GetShip(ShipType shipType) 
    {
        return (shipType != ShipType.None) ? Ships[(int)shipType - 1] : null;
    }

    //�׽�Ʈ =========================================

#if UNITY_EDITOR
    public void Test_IsSuccessLine(Vector2Int grid) 
    {
        if(IsSuccessLine(grid, lastSuccessAttackPos, true)) 
        {
            Debug.Log("������� ����");
        }
        else if (IsSuccessLine(grid, lastSuccessAttackPos, false))
        {
            Debug.Log("�������� ����");
        }
        else 
        {
            Debug.Log("�� �� �ƴ�");
        }
    }
#endif
}
