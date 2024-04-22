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

    const string BlankUserName = "□□□□□□□□";
    const string BlankPlayersInGame = "-";

    private void Start()
    {
        Transform c = transform.GetChild(0);
        Button startHosh = c.GetComponent<Button>();
        startHosh.onClick.AddListener(() => 
        {           //네트워크 매니져는 싱글톤임, 스타트호스트가 불 리턴인이유는 호스트가 이미 방을 만들어둔 상태라면 또다른 방을 만드는 시도가 실패로 끝나야 하기때문  
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("호스트 성공");
            }
            else 
            {
                Debug.Log("호스트 실패");
            }
        });

        c = transform.GetChild(1);
        Button startClient = c.GetComponent<Button>();
        startClient.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient()) //방이 만들어지지 않았거나 네트워크 연결에 실패시
            {
                Debug.Log("클라이언트 연결 성공");
            }
            else 
            {
                Debug.Log("클라이언트 연결 실패");
            }
        });

        c = transform.GetChild (2);
        Button disconnect = c.GetComponent<Button>();
        disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();    //자신의 연결 종료
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
