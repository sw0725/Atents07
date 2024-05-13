using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipsInfo : MonoBehaviour
{
    public PlayerBase player;

    TextMeshProUGUI[] text;
    private void Awake()
    {
        text = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Ship[] ships = player.Ships;
        for (int i = 0; i < ships.Length; i++) 
        {
            PrintHP(text[i], ships[i]);
            //ships[i].onHit += (ship) => PrintHP(text[i], ship);     //델리게이트 의 경우 연결한 함수가 트리거의 뒤에 붙어(for문 밖) 실행되는 경우이므로 이런식으로 지역변수(i)를 넣으면 참조타입으로 저장하려 들기 때문에 메모리 낭비가 심하다.
            int index = i;
            ships[i].onHit += (ship) => PrintHP(text[index], ship);
            ships[i].onSink += (_) => 
            {
                text[index].fontSize = 40.0f;
                text[index].text = "<#ff0000>Destroy!!</color>";
            };
        }
    }

    void PrintHP(TextMeshProUGUI text, Ship ship) 
    {
        text.text = $"{ship.HP}/{ship.Size}";
    }
}
