using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultAnalysis : MonoBehaviour
{
    TextMeshProUGUI[] texts;

    private void Awake()
    {
        Transform c = transform.GetChild(1);
        texts = c.GetComponentsInChildren<TextMeshProUGUI>();
    }

    public int AllAttackCount 
    {
        set 
        {
            texts[0].text = $"<b>{value}</b> ȸ";
        }
    }

    public int successAttackCount 
    {
        set
        {
            texts[1].text = $"<b>{value}</b> ȸ";
        }
    }

    public int FailAttackCount 
    {
        set
        {
            texts[2].text = $"<b>{value}</b> ȸ";
        }
    }

    public float SuccessAttackRate 
    {
        set
        {
            texts[3].text = $"<b>{(value * 100.0f) : f1}</b> %";
        }
    }
}
