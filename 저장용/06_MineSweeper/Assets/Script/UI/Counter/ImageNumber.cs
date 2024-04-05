using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImage;
    Sprite ZeroSprite => numberImage[0];            //�ڵ� ������ ����
    Sprite MinusSprite => numberImage[11];
    Sprite EmptySprite => numberImage[10];

    Image[] numberDigits; 


    public int Number 
    {
        get => number;
        set 
        {
            if(number != value) 
            {
                number = Mathf.Clamp(value, -99, 999);
                Refreash();
            }
        }
    }
    int number = -1;

    private void Awake()
    {
        numberDigits = GetComponentsInChildren<Image>();
    }

    void Refreash() 
    {
        int temp = Mathf.Abs(Number);
        Queue<int> digits = new Queue<int>(3);
        while(temp > 0) 
        {                                //������ ���ڸ�(1���ڸ�) �ϳ����� ť�� �ֱ�
            digits.Enqueue(temp % 10);   //���ڸ����� �ΰ� ���� ���ڸ����� �Ѱ� ��
            temp /= 10;
        }

        int index = 0;
        while(digits.Count > 0) 
        {                                //�ڸ���(1���ڸ�)������ ť�� ������ �ֱ�
            int num = digits.Dequeue();
            numberDigits[index].sprite = numberImage[num];
            index++;                     //���������� ť�� �� ����ŭ ���� == �ڸ����� ����
        }

        for (int i = index; i < numberDigits.Length; i++) //�ڸ��� �̿� ���� �κ��� 0����
        {
            numberDigits[i].sprite = ZeroSprite;
        }

        if (Number < 0) 
        {
            numberDigits[numberDigits.Length - 1].sprite = MinusSprite; //�� ���ڸ��� ���̳ʽ�
        }
    }
}
