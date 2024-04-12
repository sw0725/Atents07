using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI rank;
    TextMeshProUGUI rankText;
    TextMeshProUGUI record;
    TextMeshProUGUI recordText;
    TextMeshProUGUI rankerName;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        rank = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(1);
        rankText = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(2);
        record = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(3);
        recordText = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild (4);
        rankerName = c.GetComponent<TextMeshProUGUI>();
    }

    public void SetData<T>(int rankData, T recordData, string nameData)//���ʸ�Ÿ���� �Լ����� Ư�� �κп��� ������ ���� �ִ� == ���ʸ��Լ�(�� ���� ������ Ÿ�Ը� �޶����� ��� ���)
    { 
        rank.text = rankData.ToString();
        if (recordData.GetType() == typeof(float))
        {
            record.text = $"{recordData:f1}";                           //�Ҽ��� ���ڸ��� ���
        }
        else 
        {
            record.text = recordData.ToString();
        }
        rankerName.text = nameData;

        rankText.enabled = true;
        recordText.enabled = true;
    }

    public void ClearLine() 
    {
        rank.text = string.Empty;
        record.text = string.Empty;
        rankerName.text = string.Empty;

        rankText.enabled = false;
        recordText.enabled = false;
    }
}
