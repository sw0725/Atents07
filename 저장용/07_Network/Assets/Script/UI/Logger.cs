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
            if (text[0] == '/')                                     //庚切伸税 析採研 搾嘘馬澗暗虞 戚禎什亜 照股毘
            {
                ConsoleCommand(text);
                Debug.Log("in");
            }
            else 
            {
                if (GameManager.Instance.Player != null)
                {
                    GameManager.Instance.Player.SendChat(text);
                }
                else
                {
                    Log(text);
                }
            }

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

    void ConsoleCommand(string command) 
    {
        //湛切澗/, 誤敬嬢 姥歳, /setname sdsa UserName => sdsa, /setcolor 1,0,0 事雌 => 降娃事
        int space = command.IndexOf(' ');
        string commandToken = command.Substring(0, space);      //昔畿什採斗 庚切蒋 猿走税 庚切伸 鋼発
        commandToken = commandToken.ToLower();
        string dataToken = command.Substring(space + 1);        //庚切陥製牒採斗 魁猿走

        GameManager gameManager = GameManager.Instance;
        switch (commandToken) 
        {
            case "/setname":
                gameManager.UserName = dataToken;
                if (gameManager.Decorator != null) gameManager.Decorator.SetName(dataToken);
                break;
            case "/setcolor":
                string[] colorString = dataToken.Split(',', ' ');
                float[] colorValues = new float[3] { 0, 0, 0 };

                int count = 0;
                foreach(string color in colorString) 
                {
                    if(color.Length == 0) continue;
                    if (count > 2) break;

                    if(!float.TryParse(color, out colorValues[count])) colorValues[count] = 0;
                    count++;
                }

                for(int i = 0; i < colorValues.Length; i++) 
                {
                    colorValues[i] = Mathf.Clamp01(colorValues[i]);
                }

                Color resultColor = new Color(colorValues[0], colorValues[1], colorValues[2]);
                if(gameManager.Decorator != null) gameManager.Decorator.SetColor(resultColor);
                    
                gameManager.UserColor = resultColor;

                break;
        }
    }
}
