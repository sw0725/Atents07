using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNetController : MonoBehaviour
{
    TextMeshProUGUI playerInGame;
    TextMeshProUGUI userName;

    const string BlankUserName = "���������";
    const string BlankPlayersInGame = "-";

    private void Start()
    {
        Transform c = transform.GetChild(0);
        Button startHosh = c.GetComponent<Button>();
        startHosh.onClick.AddListener(() => 
        {           //��Ʈ��ũ �Ŵ����� �̱�����, ��ŸƮȣ��Ʈ�� �� ������������ ȣ��Ʈ�� �̹� ���� ������ ���¶�� �Ǵٸ� ���� ����� �õ��� ���з� ������ �ϱ⶧��  
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("ȣ��Ʈ ����");
            }
            else 
            {
                Debug.Log("ȣ��Ʈ ����");
            }
        });

        c = transform.GetChild(1);
        Button startClient = c.GetComponent<Button>();
        startClient.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient()) //���� ��������� �ʾҰų� ��Ʈ��ũ ���ῡ ���н�
            {
                Debug.Log("Ŭ���̾�Ʈ ���� ����");
            }
            else 
            {
                Debug.Log("Ŭ���̾�Ʈ ���� ����");
            }
        });

        c = transform.GetChild (2);
        Button disconnect = c.GetComponent<Button>();
        disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();    //�ڽ��� ���� ����
        });

        c = transform.GetChild(3);
        c = c.GetChild(1);
        playerInGame = c.GetComponent<TextMeshProUGUI>();

        GameManager gameManager = GameManager.Instance;
        gameManager.onPlayersInGameChange += (count) => playerInGame.text = count.ToString();

        c = transform.GetChild(4);
        c = c.GetChild(1);
        userName = c.GetComponent<TextMeshProUGUI>();
        gameManager.onUserNameChange += (name) =>
        {
            userName.text = gameManager.UserName;
        };

        gameManager.onPlayerDisconnected += () =>
        {
            userName.text = BlankUserName;
            playerInGame.text = BlankPlayersInGame;
        };
    }
}
