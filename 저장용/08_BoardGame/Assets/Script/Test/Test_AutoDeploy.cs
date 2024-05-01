using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test_AutoDeploy : Test_ShipDeploy
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;

    protected override void Start()
    {
        base.Start();

        reset.onClick.AddListener(ClearBoard);
        random.onClick.AddListener(AutoShipDeployment);
        resetAndRandom.onClick.AddListener(ClearBoard);
        resetAndRandom.onClick.AddListener(AutoShipDeployment);
    }

    protected void AutoShipDeployment()
    {
        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> high = new List<int>(maxCapacity);
        List<int> low = new List<int>(maxCapacity);

        for(int i = 0; i<maxCapacity; i++) 
        {
            if ((i % Board.BoardSize == 0) || (i % Board.BoardSize == (Board.BoardSize -1)) ||(i > 0 && i<Board.BoardSize-1) || (Board.BoardSize * (Board.BoardSize-1)<i && i< Board.BoardSize * Board.BoardSize - 1)) //������ ��ǥ���ε����� ��ȯ�� �з��������� (0,10,20...) = (x = 10 y = i)||(9,19,29...)||(1,2,3...)||(91,92,93...)
            {
                low.Add(i); //�׵θ�
            }
            else 
            { 
                high.Add(i);
            }
        }

        foreach(var ship in ships) 
        {
            if (ship.IsDeployed) 
            {
                int[] shipIndice = new int[ship.Size];
                for(int i = 0; i < ship.Size; i++) 
                {
                    shipIndice[i] = board.GridToIndex(ship.Positions[i]).Value;
                }
                foreach (var index in shipIndice)                               //�谡 ��ġ�Ȱ� ����
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPos(ship);                       //��ġ �ֺ��� ���ϱ�
                foreach(var index in toLow) 
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

        foreach(var ship in ships) 
        {
            if(!ship.IsDeployed) 
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
                        for(int i = 1; i< shipPos.Length; i++) 
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
                }while(fail && count < 10 && high.Count > 0);

                count = 0;
                while(fail && count < 1000) 
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
                ship.gameObject.SetActive(true);

                List<int> tempList = new List<int>(shipPos.Length);
                foreach(var pos in shipPos) 
                {
                    tempList.Add(board.GridToIndex(pos).Value);
                }
                foreach(var index in tempList) 
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

    void ClearBoard()
    {
        board.ResetBoard(ships);
    }
}