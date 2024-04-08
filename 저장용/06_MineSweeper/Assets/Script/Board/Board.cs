using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    public GameObject cellPrefeb;
    public Sprite[] openCellImage;
    public Sprite this[OpenCellType type] => openCellImage[(int)type];

    public Sprite[] closeCellImage;
    public Sprite this[CloseCellType type] => closeCellImage[(int)type];

    int width = 16;
    int height = 16;
    int mineCount = 10;

    Cell[] cells;
    Cell currentCell = null;
    Cell CurrentCell 
    {
        get => currentCell;
        set 
        {
            if (currentCell != value) 
            {
                currentCell?.RestoreCover();
                currentCell = value;
                currentCell?.LeftPress();
            }
        }
    }
    PlayerInputAction action;
    GameManager gameManager;

    const float Distance = 1.0f;

    private void Awake()
    {
        action = new PlayerInputAction();
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.LeftClick.performed += OnLeftPress;
        action.Player.LeftClick.canceled += OnLeftRelease;
        action.Player.RightClick.performed += OnRightClick;
        action.Player.MouseMove.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        action.Player.LeftClick.performed -= OnLeftPress;
        action.Player.LeftClick.canceled -= OnLeftRelease;
        action.Player.RightClick.performed -= OnRightClick;
        action.Player.MouseMove.performed -= OnMouseMove;
        action.Player.Disable();
    }

    public void Initialize(int newWidth, int newHieght, int newMineCount)
    {
        gameManager = GameManager.Instance;

        width = newWidth;
        height = newHieght;
        mineCount = newMineCount;

        cells = new Cell[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefeb, transform);
                Cell cell = cellObj.GetComponent<Cell>();

                int id = x + y * width;
                cell.ID = id;
                cell.transform.localPosition = new Vector3(x * Distance, -y * Distance);
                cell.Board = this;

                cell.onFlagUse += gameManager.DecreaseFlagCount;
                cell.onFlagReturn += gameManager.IncreaseFlagCount;

                cellObj.name = $"Cell_{id}_({x},{y})";
                cells[id] = cell;
            }
        }

        foreach (Cell cell in cells) 
        {
            cell.InitialIze();
        }

        ResetBoard();
    }

    private void ResetBoard()
    {
        foreach (Cell cell in cells)
        {
            cell.ResetData();
        }
        Shuffle(cells.Length, out int[] suffleResult);
        for (int i = 0; i < mineCount; i++) 
        {
            cells[suffleResult[i]].SetMine();
        }
    }

    //  셀확인용=============================================

    Vector2Int ScreenToGrid(Vector2 screen)
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(screen);
        Vector2 diff = world - (Vector2)transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x / Distance), Mathf.FloorToInt(-diff.y / Distance));
    }

    int? GridToIndex(int x, int y)
    {
        int? result = null;
        if (IsValidGrid(x, y))
        {
            result = (x + (y * height));
        }
        return result;
    }

    Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index/ width);
    }

    bool IsValidGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height && cells != null;
    }

    Cell GetCell(Vector2 screen)
    {
        Cell result = null;
        Vector2Int grid = ScreenToGrid(screen);
        int? index = GridToIndex(grid.x, grid.y);
        if (index != null)
        {
            result = cells[index.Value];
        }
        return result;
    }

    //  입력처리=====================================================
    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (Mouse.current.leftButton.isPressed)                     //마우스 왼버튼 눌렸나
        {
            Vector2 screen = context.ReadValue<Vector2>();
            Cell cell = GetCell(screen);
            CurrentCell = cell;
        }
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Cell cell = GetCell(screen);
        cell?.CellRightPress();
    }

    private void OnLeftRelease(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Cell cell = GetCell(screen);
        cell?.LeftRelease();
    }

    private void OnLeftPress(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Cell cell = GetCell(screen);
        cell?.LeftPress();
    }

    //  기타 유틸리티=============================================

    void Shuffle(int count, out int[] result) 
    {
        result = new int[count];
        for(int i = 0; i < count; i++) 
        {
            result[i] = i;
        }

        int loopCount = result.Length-1;
        for (int i = 0; i < loopCount; i++) 
        {
            int rand = Random.Range(0, result.Length -i );
            int lastIndex = loopCount - i;
            (result[lastIndex], result[rand]) = (result[rand], result[lastIndex]);
        }
    }

    public List<Cell> GetNeightbors(int id) 
    {
        List<Cell> result = new List<Cell>();
        Vector2Int grid = IndexToGrid(id);
        for (int y = -1; y < 2; y++) 
        {
            for (int x = -1; x < 2; x++)
            {
                if (!(x == 0 && y == 0)) 
                {
                    int? index = GridToIndex(x + grid.x, y + grid.y);
                    if (index != null) 
                    {
                        result.Add(cells[index.Value]);
                    }
                }
            }
        }
        return result;
    }

#if UNITY_EDITOR
    public void Test_OpenAllCover()
    {
        foreach(Cell cell in cells) 
        {
            cell.Test_OpenCover();
        }
    }

    public void Test_BoardReset() 
    {
        ResetBoard();
    }
#endif
}
