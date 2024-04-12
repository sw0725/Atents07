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

    public void SetData<T>(int rankData, T recordData, string nameData)//제너릭타입을 함수안의 특정 부분에만 지정할 수도 있다 == 제너릭함수(할 일은 같은데 타입만 달라지는 경우 사용)
    { 
        rank.text = rankData.ToString();
        if (recordData.GetType() == typeof(float))
        {
            record.text = $"{recordData:f1}";                           //소수전 한자리만 출력
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
