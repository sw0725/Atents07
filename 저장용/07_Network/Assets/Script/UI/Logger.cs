using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public Color warningColor;      //노랑
    public Color errorColor;        //빨강

    StringBuilder sb;
    TextMeshProUGUI log;
    TMP_InputField inputField;
    Queue<string> logLines = new Queue<string>(MaxLineCount +1);

    const int MaxLineCount = 20;

    private void Awake()
    {
        Transform c = transform.GetChild(3);
        inputField = c.GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener((text) =>                 //입력이 완료(엔터)되었을때 실행, endEdit:입력이 끝났을때(엔터 or 포커스종료) 실행
        {
            Log(text);
            inputField.text = string.Empty;
            inputField.ActivateInputField();                     //입력완료 후 포커스 다시 활성화, Select() : 활성화시 비활성화 / 비활성화시 활성화
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

    public void Log(string message)         //{255.255.0}[255.0.0]출력
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
            sb.AppendLine(line);            //Append : 추가 AppendLine : 줄 추가(\n 자동으로 붙음)
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
            string[] split = source.Split(open, close);             //이경우 ㅁㅁㅁ{ㄴㄴㄴ}{ㅇㅇㅇ}일때 ㅁㅁㅁ, ㄴㄴㄴ, ,ㅇㅇㅇ 로 쪼갠다
            string colortext = ColorUtility.ToHtmlStringRGB(color);
            result = string.Empty;

            for(int i = 0; i < split.Length; i++) 
            {
                result += split[i];
                if(i < split.Length - 1) 
                {
                    if (i % 2 == 0)                                 //짝수번째 블록 즉 '{' 앞서 ㅁㅁㅁ,{ㄴㄴㄴ}, ,{ㅇㅇㅇ}로 나누어지기에 이 뒤로 { 가 시작됨
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
        if (count % 2 != 0 || count == 0)               //{, }는 한쌍이므로 카운트값은 언제나 짝수다.
        {
            result = false;
        }
        return result;
    }
}
