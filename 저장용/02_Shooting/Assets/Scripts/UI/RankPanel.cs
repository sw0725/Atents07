using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines;
    int[] highScore;
    string[] rankerName;

    const int rankCount = 5;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        highScore = new int[rankCount];
        rankerName = new string[rankCount];
    }

    void SetDefaultData() 
    {
        for (int i = 0; i < rankCount; i++) 
        {                                   //아스키코드사용
            char name = (char)(65 + i);     //((char)((char)0x0041 + i))-유니코드 temp = 'a'; (char)((byte)temp + i)-아스키코드사용
            rankerName[i] = $"{name}{name}{name}";
            int score = (int)Mathf.Pow(10, (rankCount - i));        //이거 무거운 함수임 자주실행된다면 차라리 for로 돌리는게
            highScore[i] = score;                                   //for (int j=rankCount; j>0; j--) {score *= 10}
        }
        RefreshRankLine();
    }

    void SaveRankData()
    {
        SaveData data = new SaveData();                                //데이터 저장형식
        data.rankerName = rankerName;
        data.highScore = highScore;
        string jsonText = JsonUtility.ToJson(data);                   //저장정보를 제이슨으로 변형

                                  //~아래의 세이브폴더 아래에저장       //Application.dataPath = 에셋폴더가 있는위치 ==실행파일있는위치
        string path = $"{Application.dataPath}/Save/";               //@"\Save" = 폴더명으로 직접지목 @=폴더표시
        if (!Directory.Exists(path))                                 //폴더있->t 폴더없->f
        {
            Directory.CreateDirectory(path);                        //거따가 만들기 단 중간 경로 명시해야함
        }
                //구식방법 - 데이터를 스필릿으로 일일이 분해해줘야함
        //string fullPath = $"{path}Save.txt";                         // 세이브파일 변형이싫으면 Text말고 바이너리로 저장한다
        //System.IO.File.WriteAllText(fullPath, "SaveGame");          //이 함수의 경로는 위치경로 + 파일이름 이 필요하다 ->이경우는 치팅/모딩가능

        string fullPath = $"{path}Save.json";                           //제이슨형식으로 파일지정
        System.IO.File.WriteAllText(fullPath, jsonText);
    }

    bool LoadRankData() 
    {
        bool result = false;    //성공여부
        return result;
    }

    void UpdateRankData(int Score)
    {
        
    }

    void RefreshRankLine() 
    {
        for (int i = 0; i < rankCount; i++)
        {
            rankLines[i].SetData(rankerName[i], highScore[i]);
        }
    }

    public void Test_RankPanel() 
    {
        SetDefaultData();
        RefreshRankLine();
    }

    public void Test_SaveRankPanel()
    {
        SetDefaultData();
        RefreshRankLine();
        SaveRankData();
    }
}
