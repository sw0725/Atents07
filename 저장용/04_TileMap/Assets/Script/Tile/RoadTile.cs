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
    
    [Flags]         //이 이넘을 비트플래그로 사용한다고 표시하는 어트리뷰트
    enum AdjTilePosition : byte                                               //비트맵은 이넘으로 만드는게 좋다
    {                     //이 이넘의 크기는 1바이트다
        None = 0,   //0000 0000
        North = 1,  //0000 0001
        East = 2,   //0000 0010
        South = 4,  //0000 0100
        West = 8,   //0000 1000
        All = North | East | South | West   //0000 1111
    }
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)                     //타일이 그려질때 실행
    { //주변 같은종류 타일 갱신이 목적      //타일의 위치(그리드좌표)   //타일이 그려질 타일맵
        for(int y = -1; y < 2; y++) 
        {
            for (int x = -1; x < 2; x++)                    //나 중심 3*3공간에서
            {
                Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasThisTile(tilemap, location))         //같은 타일이있으면
                {
                    tilemap.RefreshTile(location);          //리프레쉬
                }
            }
        }
    }
                    //리프레쉬 호출시 호출       //무엇이 그려질지 결정               //타일데이터는 구조체(값타입)
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {//사방 확인 후 확인 내용을 저장(비트플래그) //ref = 읽기와 쓰기 전부 가능(구조체도 참조형으로 가져옴) => 원본을 같이 수정
        AdjTilePosition mask = AdjTilePosition.None;
             //a = a||b                                               //삼항연산자
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;
            //이미지선택
        int index = GetIndex(mask);
        if (index > -1 && index < sprites.Length)                              //인덱스가 제대로 골라지면
        {
            tileData.sprite = sprites[index];                                  //스프라이트 설정
            Matrix4x4 matrix = tileData.transform;
            matrix.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);       //타일회전
            tileData.transform = matrix;
            tileData.flags = TileFlags.LockTransform;                          //고정,외부개입방지
        }
        else 
        {
            Debug.LogError($"잘못된 인덱스 : {index}, mask = {mask}");
        }
    }

    bool HasThisTile(ITilemap tilemap, Vector3Int position) 
    {
        return tilemap.GetTile(position) == this;   //특정타일맵에서 해당위치에 이 타일과 같은 종류의 타일이 있는가
    }

    int GetIndex(AdjTilePosition mask) //마스크 값에따라 그려야할 스프라이트의 인덱스 리턴 
    {
        int index = -1;

        switch (mask) 
        {
            case AdjTilePosition.None:
            case AdjTilePosition.North:
            case AdjTilePosition.South:
            case AdjTilePosition.West:
            case AdjTilePosition.East:
            case AdjTilePosition.North | AdjTilePosition.South: //북남
            case AdjTilePosition .West | AdjTilePosition .East: //동서
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

#if UNITY_EDITOR                                                    //에셋우클릭-크리에이트-2d-...
    [MenuItem("Assets/Create/2D/Tiles/Custom/RoadTile")]  //사용할 명령어 주소
    public static void CreateTiles() 
    {
        string path = EditorUtility.SaveFilePanelInProject(     //파일 저장창 열고 입력결과 반환
                "Save Road Tile",                               //제목
                "New Road Tile",                                //디폴트파일명
                "Asset",                                        //파일의 디폴트 확장자
                "Save Road Tile",                               //출력용 메세지
                "Asstes/Tiles"                                  //열릴 기본폴더
            );
        if (path != null) 
        {
            AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    //RoadTile을 파일로 저장
        }
    }
#endif
}
