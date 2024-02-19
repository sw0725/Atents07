using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile
{
    public Sprite[] sprites;
    
    [Flags]         //�� �̳��� ��Ʈ�÷��׷� ����Ѵٰ� ǥ���ϴ� ��Ʈ����Ʈ
    enum AdjTilePosition : byte                                               //��Ʈ���� �̳����� ����°� ����
    {                     //�� �̳��� ũ��� 1����Ʈ��
        None = 0,   //0000 0000
        North = 1,  //0000 0001
        East = 2,   //0000 0010
        South = 4,  //0000 0100
        West = 8,   //0000 1000
        All = North | East | South | West   //0000 1111
    }
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)                     //Ÿ���� �׷����� ����
    { //�ֺ� �������� Ÿ�� ������ ����      //Ÿ���� ��ġ(�׸�����ǥ)   //Ÿ���� �׷��� Ÿ�ϸ�
        for(int y = -1; y < 2; y++) 
        {
            for (int x = -1; x < 2; x++)                    //�� �߽� 3*3��������
            {
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasThisTile(tilemap, location))         //���� Ÿ����������
                {
                    tilemap.RefreshTile(location);          //��������
                }
            }
        }
    }
                    //�������� ȣ��� ȣ��       //������ �׷����� ����               //Ÿ�ϵ����ʹ� ����ü(��Ÿ��)
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {//��� Ȯ�� �� Ȯ�� ������ ����(��Ʈ�÷���) //ref = �б�� ���� ���� ����(����ü�� ���������� ������) => ������ ���� ����
        AdjTilePosition mask = AdjTilePosition.None;
             //a = a||b                                               //���׿�����
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;
            //�̹�������
        int index = GetIndex(mask);
        if (index > -1 && index < sprites.Length)                              //�ε����� ����� �������
        {
            tileData.sprite = sprites[index];                                  //��������Ʈ ����
            Matrix4x4 matrix = tileData.transform;
            matrix.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);       //Ÿ��ȸ��
            tileData.transform = matrix;
            tileData.flags = TileFlags.LockTransform;                          //����,�ܺΰ��Թ���
        }
        else 
        {
            Debug.LogError($"�߸��� �ε��� : {index}, mask = {mask}");
        }
    }

    bool HasThisTile(ITilemap tilemap, Vector3Int position) 
    {
        return tilemap.GetTile(position) == this;   //Ư��Ÿ�ϸʿ��� �ش���ġ�� �� Ÿ�ϰ� ���� ������ Ÿ���� �ִ°�
    }

    int GetIndex(AdjTilePosition mask) //����ũ �������� �׷����� ��������Ʈ�� �ε��� ���� 
    {
        int index = -1;

        switch (mask) 
        {
            case AdjTilePosition.None:
            case AdjTilePosition.North:
            case AdjTilePosition.South:
            case AdjTilePosition.West:
            case AdjTilePosition.East:
            case AdjTilePosition.North | AdjTilePosition.South: //�ϳ�
            case AdjTilePosition .West | AdjTilePosition .East: //����
                index = 0;
                break;
            case AdjTilePosition.North | AdjTilePosition.West:
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.South | AdjTilePosition.West:
            case AdjTilePosition.South | AdjTilePosition.East:
                index = 1;
                break;
            case AdjTilePosition.All & ~AdjTilePosition.North:                        //~ = not
            case AdjTilePosition.All & ~AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.South:      //(0000 1111) & ~(0000 0100) = 0000 1011
            case AdjTilePosition.All & ~AdjTilePosition.West:
                index = 2;
                break;
            case AdjTilePosition.All:
                index = 3;
                break;
        }

        return index;
    }

    Quaternion GetRotation(AdjTilePosition mask) 
    {
        Quaternion rotation = Quaternion.identity;

        switch (mask) 
        {
            case AdjTilePosition.East:
            case AdjTilePosition.West:
            case AdjTilePosition.West | AdjTilePosition.East:
            case AdjTilePosition.North | AdjTilePosition.West:
            case AdjTilePosition.All & ~AdjTilePosition.West:
                rotation = Quaternion.Euler(0, 0, -90);
                break;
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.North:
                rotation = Quaternion.Euler(0, 0, -180);
                break;
            case AdjTilePosition.South | AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.East:
                rotation = Quaternion.Euler(0, 0, -270);
                break;
        }

        return rotation;
    }

#if UNITY_EDITOR                                                    //���¿�Ŭ��-ũ������Ʈ-2d-...
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]  //����� ��ɾ� �ּ�
    public static void CreateTiles() 
    {
        string path = EditorUtility.SaveFilePanelInProject(     //���� ����â ���� �Է°�� ��ȯ
                "Save Road Tile",                               //����
                "New Road Tile",                                //����Ʈ���ϸ�
                "Asset",                                        //������ ����Ʈ Ȯ����
                "Save Road Tile",                               //��¿� �޼���
                "Asstes/Tiles"                                  //���� �⺻����
            );
        if (path != null) 
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    //RoadTile�� ���Ϸ� ����
        }
    }
#endif
}
