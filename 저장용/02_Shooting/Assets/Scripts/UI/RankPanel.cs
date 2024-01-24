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

        inputField = GetComponentInChildren<TMP_InputField>(true);  //true = ��Ȱ��ȭ�� ã��
        inputField.onEndEdit.AddListener(OnNameInputEnd);          //UI�� ��������Ʈ = on~�ø���
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
        {                                   //�ƽ�Ű�ڵ���
            char name = (char)(65 + i);     //((char)((char)0x0041 + i))-�����ڵ� temp = 'a'; (char)((byte)temp + i)-�ƽ�Ű�ڵ���
            rankerName[i] = $"{name}{name}{name}";
            int score = (int)Mathf.Pow(10, (rankCount - i));        //�̰� ���ſ� �Լ��� ���ֽ���ȴٸ� ���� for�� �����°�
            highScore[i] = score;                                   //for (int j=rankCount; j>0; j--) {score *= 10}
        }
        RefreshRankLine();
    }

    void SaveRankData()
    {
        SaveData data = new SaveData();                                //������ ��������
        data.rankerName = rankerName;
        data.highScore = highScore;
        string jsonText = JsonUtility.ToJson(data);                   //���������� ���̽����� ����

                                  //~�Ʒ��� ���̺����� �Ʒ�������       //Application.dataPath = ���������� �ִ���ġ ==���������ִ���ġ
        string path = $"{Application.dataPath}/Save/";               //@"\Save" = ���������� �������� @=����ǥ��
        if (!System.IO.Directory.Exists(path))                                 //������->t ������->f
        {
            System.IO.Directory.CreateDirectory(path);                        //�ŵ��� ����� �� �߰� ��� ����ؾ���
        }
                //���Ĺ�� - �����͸� ���ʸ����� ������ �����������
        //string fullPath = $"{path}Save.txt";                         // ���̺����� �����̽����� Text���� ���̳ʸ��� �����Ѵ�
        //System.IO.File.WriteAllText(fullPath, "SaveGame");          //�� �Լ��� ��δ� ��ġ��� + �����̸� �� �ʿ��ϴ� ->�̰��� ġ��/�������

        string fullPath = $"{path}Save.json";                           //���̽��������� ��������
        System.IO.File.WriteAllText(fullPath, jsonText);
    }

    bool LoadRankData()
    {
        bool result = false;    //��������
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
