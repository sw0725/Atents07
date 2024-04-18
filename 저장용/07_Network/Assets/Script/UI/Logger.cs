using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public Color warningColor;      //葛櫛
    public Color errorColor;        //察悪

    StringBuilder sb;
    TextMeshProUGUI log;
    TMP_InputField inputField;
    Queue<string> logLines = new Queue<string>(MaxLineCount +1);

    const int MaxLineCount = 20;

    private void Awake()
    {
        Transform c = transform.GetChild(3);
        inputField = c.GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener((text) =>                 //脊径戚 刃戟(殖斗)鞠醸聖凶 叔楳, endEdit:脊径戚 魁概聖凶(殖斗 or 匂朕什曽戟) 叔楳
        {
            Log(text);
            inputField.text = string.Empty;
            inputField.ActivateInputField();                     //脊径刃戟 板 匂朕什 陥獣 醗失鉢, Select() : 醗失鉢獣 搾醗失鉢 / 搾醗失鉢獣 醗失鉢
        });                                 

        c = transform.GetChild(0);
        c = c.GetChild(0);
        c = c.GetChild(0);
        log = c.GetComponent<TextMeshProUGUI>();

        sb = new StringBuilder(MaxLineCount +1);
    }

    private void Start()
    {
        log.text = string.Empty;
    }

    public void Log(string message)         //{255.255.0}[255.0.0]窒径
    {
        message = HighlightSubString(message, '[', ']', errorColor);
        message = HighlightSubString(message, '{', '}', warningColor);

        logLines.Enqueue(message);
        if (logLines.Count > MaxLineCount)
        {
            logLines.Dequeue();
        }

        sb.Clear();
        foreach (string line in logLines) 
        {
            sb.AppendLine(line);            //Append : 蓄亜 AppendLine : 匝 蓄亜(\n 切疑生稽 細製)
        }

        log.text = sb.ToString();
    }

    public void InputFieldFocusOn()
    {
        inputField.ActivateInputField();
    }

    string HighlightSubString(string source, char open, char close, Color color) 
    {
        string result = source;
        if(IsPair(source, open, close)) 
        {
            string[] split = source.Split(open, close);             //戚井酔 けけけ{いいい}{ししし}析凶 けけけ, いいい, ,ししし 稽 舵圧陥
            string colortext = ColorUtility.ToHtmlStringRGB(color);
            result = string.Empty;

            for(int i = 0; i < split.Length; i++) 
            {
                result += split[i];
                if(i < split.Length - 1) 
                {
                    if (i % 2 == 0)                                 //側呪腰属 鷺系 聡 '{' 蒋辞 けけけ,{いいい}, ,{ししし}稽 蟹刊嬢走奄拭 戚 及稽 { 亜 獣拙喫
                    {
                        result += $"<#{colortext}>";
                    }
                    else 
                    {
                        result += "</color>";
                    }
                }
            }
        }

        return result;
    }

    bool IsPair(string source, char open, char close) 
    {
        bool result = true;
        int count = 0;
        for (int i = 0; i < source.Length; i++) 
        {
            if (source[i] == open || source[i] == close) 
            {
                count++;

                if (count % 2 == 1)
                {
                    if (source[i] != open)
                    {
                        result = false;
                        break;
                    }
                }
                else 
                {
                    if (source[i] != close) 
                    {
                        result = false;
                        break;
                    }
                }
            }
        }
        if (count % 2 != 0 || count == 0)               //{, }澗 廃瞬戚糠稽 朝錘闘葵精 情薦蟹 側呪陥.
        {
            result = false;
        }
        return result;
    }
}
