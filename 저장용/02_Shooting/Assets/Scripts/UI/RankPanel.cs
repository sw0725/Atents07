using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines;
    int[] highScore;
    string[] rankerName;

    const int rankCount = 5;
    TMP_InputField inputField;
    int updatedIndex = -1;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>(true);
        highScore = new int[rankCount];
        rankerName = new string[rankCount];

        inputField = GetComponentInChildren<TMP_InputField>(true);  //true = 비활성화도 찾기
        inputField.onEndEdit.AddListener(OnNameInputEnd);          //UI용 델리게이트 = on~시리즈
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onDead += UpdateRankData;
        LoadRankData();
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
        if (!System.IO.Directory.Exists(path))                                 //폴더있->t 폴더없->f
        {
            System.IO.Directory.CreateDirectory(path);                        //거따가 만들기 단 중간 경로 명시해야함
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
        string path = $"{Application.dataPath}/Save/";
        if (System.IO.Directory.Exists(path))
        {
            string fullPath = $"{path}Save.json";
            if (System.IO.File.Exists(fullPath))
            {
                string json = System.IO.File.ReadAllText(fullPath);

                SaveData data = JsonUtility.FromJson<SaveData>(json);
                highScore = data.highScore;
                rankerName = data.rankerName;

                result = true;
            }
        }

        if (!result) 
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            SetDefaultData();
        }
        
        RefreshRankLine();
        return result;
    }

    void UpdateRankData(int Score)
    {
        for (int i = 0;i<rankCount;i++) 
        {
            if (highScore[i] < Score) 
            {
                for (int j = rankCount - 1; j > i; j--) 
                {
                    highScore[j] = highScore[j - 1];
                    rankerName[j] = rankerName[j - 1];
                    rankLines[j].SetData(rankerName[j], highScore[j]);
                }
                highScore[i] = Score;

                rankLines[i].SetData("newRanker", Score);
                updatedIndex = i;

                Vector3 pos = inputField.transform.position;
                pos.y = rankLines[i].transform.position.y;
                inputField.transform.position = pos;

                inputField.gameObject.SetActive(true);

                break;
            }
        }
        
    }

    void RefreshRankLine() 
    {
        for (int i = 0; i < rankCount; i++)
        {
            rankLines[i].SetData(rankerName[i], highScore[i]);
        }
    }

    void OnNameInputEnd(string text)
    {
        rankerName[updatedIndex] = text;
        RefreshRankLine();
        SaveRankData();
    }
#if UNITY_EDITOR
    public void Test_RankPanel() 
    {
        SetDefaultData();
        RefreshRankLine();
    }

    public void Test_SaveRankPanel()
    {
        RefreshRankLine();
        SaveRankData();
    }
    public void Test_LoadRankPanel()
    {
        LoadRankData();
    }

    public void Test_UpdateRankPanel(int Score)
    {
        UpdateRankData(Score);
    }
# endif
}
