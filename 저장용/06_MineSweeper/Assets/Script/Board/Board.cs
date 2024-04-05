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

    int width =16;
    int height =16;
    int mineCount = 10;

    Cell[] cells;
    PlayerInputAction action;

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
        width = newWidth;
        height = newHieght;
        mineCount = newMineCount;

        cells = new Cell[width*height];

        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++) 
            {
                GameObject cellObj = Instantiate(cellPrefeb, transform);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.transform.localPosition = new Vector3(x * Distance, -y * Distance);

                int id = x + y * width;
                cell.ID = id;
                cells[id] = cell;
                cellObj.name = $"Cell_{id}_({x},{y})";
            }        
        }
        ResetBoard();
    }

    private void ResetBoard() 
    {
        foreach (Cell cell in cells) 
        {
            cell.ResetData();
        }
        List<int> before = new List<int>();
    }//     마인카운트만큼 지뢰 배치

    Vector2Int ScreenToGrid(Vector2 screen) 
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(screen);
        Vector2 diff = world -(Vector2)transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x/Distance), Mathf.FloorToInt(-diff.y/Distance));
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

    bool IsValidGrid(int x, int y) 
    {
        return x >=0 && y >= 0 && x < width && y < height;
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
        Vector2 screen = context.ReadValue<Vector2>();
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
    }

    private void OnLeftRelease(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
    }

    private void OnLeftPress(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Debug.Log(GetCell(screen).gameObject.name);
    }
#if UNITY_EDITOR
    public void Test_OpenAllCover()
    {
        foreach(Cell cell in cells) 
        {
            cell.Test_OpenCover();
        }
    }
#endif
}
