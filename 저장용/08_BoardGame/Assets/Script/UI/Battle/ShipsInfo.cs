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
            //ships[i].onHit += (ship) => PrintHP(text[i], ship);     //��������Ʈ �� ��� ������ �Լ��� Ʈ������ �ڿ� �پ�(for�� ��) ����Ǵ� ����̹Ƿ� �̷������� ��������(i)�� ������ ����Ÿ������ �����Ϸ� ��� ������ �޸� ���� ���ϴ�.
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
