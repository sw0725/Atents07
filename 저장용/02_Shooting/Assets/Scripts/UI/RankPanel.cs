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
        if (!Directory.Exists(path))                                 //������->t ������->f
        {
            Directory.CreateDirectory(path);                        //�ŵ��� ����� �� �߰� ��� ����ؾ���
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
