using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllerCell : Cell
{
    public int setGroup;
    const int NotSet = -1;
    public EllerCell(int x, int y) : base(x, y)
    {
        setGroup = NotSet;
    }
}
public class Eller : Maze
{
    [Range(0f, 1f)]
    public float mergeChance = 0.7f;
    [Range(0f, 1f)] 
    public float southOpenChance = 0.5f;
    int serial = 0;

    const float LastMergeChance = 1.1f;

    protected override void OnSpecificAlgorithmExcute()
    {
        int h = height - 1;
        EllerCell[] prevLine = null;

        for (int y =0; y < h; y++) 
        {
            EllerCell[] line = MakeLine(prevLine);

            MergeAdjacent(line, mergeChance);
            RemoveSouthWall(line);
            WriteLine(line);

            prevLine = line;
        }

        EllerCell[] lastLine = MakeLine(prevLine);
        MergeAdjacent(lastLine, LastMergeChance);
        WriteLine(lastLine);
    }

    EllerCell[] MakeLine(EllerCell[] prev) 
    {
        int row = (prev != null) ? (prev[0].Y + 1) : 0;

        EllerCell[] line = new EllerCell[Width];
        for(int x = 0; x < width; x++) 
        {
            line[x] = new EllerCell(x, row);

            if (prev != null && prev[x].IsPath(Direction.South))    //윗줄이있고 위줄에서 남쪽벽이 없다
            {
                line[x].setGroup = prev[x].setGroup;
                line[x].MakePath(Direction.North);
            }
            else                                                    //윗줄이 없거나 윗줄에서 남쪽벽이있다.
            {
                line[x].setGroup = serial;
                serial++;
            }
        }
        return line;
    }

    void MergeAdjacent(EllerCell[] line, float chance) 
    {
        int count = 1;  //한줄이 모두 같은 집합에 속하지 않도록 함 
        int w = width - 1;
        for(int x = 0; x < w; x++) 
        {
            if (count < w && line[x].setGroup != line[x + 1].setGroup && Random.value < chance) 
            {
                line[x].MakePath(Direction.East);
                line[x+1].MakePath(Direction.West);

                int targetGroup = line[x + 1].setGroup;
                line[x+1].setGroup = line[x].setGroup;

                for (int i = x + 2; i < width; i++) 
                {
                    if (line[i].setGroup == targetGroup) 
                    {
                        line[i].setGroup = line[x].setGroup;
                    }
                }

                count++;
            }
        }
    }

    void RemoveSouthWall (EllerCell[] line)
    {
        Dictionary<int, List<int>> setListDic = new Dictionary<int, List<int>>();   //키 = 집합번호, 값 =  해당 집합에 속한 셀의 x좌표 
        for (int x = 0; x < Width; x++) 
        {
            int key = line[x].setGroup;
            if (!setListDic.ContainsKey(key)) 
            {
                setListDic[key] = new List<int>();
            }
            setListDic[key].Add(x);
        }

        foreach (int key in setListDic.Keys) 
        {
            int[] array = setListDic[key].ToArray();
            Util.Shuffle(array);

            int index = array[0];
            line[index].MakePath(Direction.South);

            int length = array.Length;
            for (int i = 1; i < length; i++) 
            {
                if (Random.value < southOpenChance) 
                {
                    line[array[i]].MakePath(Direction.South);
                }
            }
        }
    }

    void WriteLine(EllerCell[] line) //Maze.cells에 저장
    {
        int index = GridToIndex(0, line[0].Y);
        for (int x = 0; x < width; x++) 
        {
            cells[index + x] = line[x];
        }
    }
}
