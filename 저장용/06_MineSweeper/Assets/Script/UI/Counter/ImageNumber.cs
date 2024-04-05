using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImage;
    Sprite ZeroSprite => numberImage[0];            //코드 가독성 위함
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
        {                                //오른쪽 끝자리(1의자리) 하나떼어 큐에 넣기
            digits.Enqueue(temp % 10);   //두자리수면 두개 들어가고 한자리수면 한개 들어감
            temp /= 10;
        }

        int index = 0;
        while(digits.Count > 0) 
        {                                //자릿수(1의자리)순으로 큐의 데이터 넣기
            int num = digits.Dequeue();
            numberDigits[index].sprite = numberImage[num];
            index++;                     //최종적으로 큐에 들어간 수만큼 증가 == 자릿수와 동일
        }

        for (int i = index; i < numberDigits.Length; i++) //자릿수 이외 남은 부분을 0세팅
        {
            numberDigits[i].sprite = ZeroSprite;
        }

        if (Number < 0) 
        {
            numberDigits[numberDigits.Length - 1].sprite = MinusSprite; //맨 앞자리에 마이너스
        }
    }
}
