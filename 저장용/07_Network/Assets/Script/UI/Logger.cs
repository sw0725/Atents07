using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public Color warningColor;      //���
    public Color errorColor;        //����

    StringBuilder sb;
    TextMeshProUGUI log;
    TMP_InputField inputField;
    Queue<string> logLines = new Queue<string>(MaxLineCount +1);

    const int MaxLineCount = 20;

    private void Awake()
    {
        Transform c = transform.GetChild(3);
        inputField = c.GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener((text) =>                 //�Է��� �Ϸ�(����)�Ǿ����� ����, endEdit:�Է��� ��������(���� or ��Ŀ������) ����
        {
            Log(text);
            inputField.text = string.Empty;
            inputField.ActivateInputField();                     //�Է¿Ϸ� �� ��Ŀ�� �ٽ� Ȱ��ȭ, Select() : Ȱ��ȭ�� ��Ȱ��ȭ / ��Ȱ��ȭ�� Ȱ��ȭ
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

    public void Log(string message)         //{255.255.0}[255.0.0]���
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
            sb.AppendLine(line);            //Append : �߰� AppendLine : �� �߰�(\n �ڵ����� ����)
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
            string[] split = source.Split(open, close);             //�̰�� ������{������}{������}�϶� ������, ������, ,������ �� �ɰ���
            string colortext = ColorUtility.ToHtmlStringRGB(color);
            result = string.Empty;

            for(int i = 0; i < split.Length; i++) 
            {
                result += split[i];
                if(i < split.Length - 1) 
                {
                    if (i % 2 == 0)                                 //¦����° ��� �� '{' �ռ� ������,{������}, ,{������}�� ���������⿡ �� �ڷ� { �� ���۵�
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
        if (count % 2 != 0 || count == 0)               //{, }�� �ѽ��̹Ƿ� ī��Ʈ���� ������ ¦����.
        {
            result = false;
        }
        return result;
    }
}
