using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetSingltrun<GameManager>
{
    public NetPlayer Player => player;

    public Action<int> onPlayersInGameChange;
    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);

    public string UserName 
    {
        get => userName;
        set 
        {
            userName = value;
            onUserNameChange?.Invoke(userName);
        }
    }
    public Action<string> onUserNameChange;
    string userName = "디폴트";

    public Color UserColor 
    {
        get => userColor;
        set 
        {
            userColor = value;
            onUserColorChange?.Invoke(userColor);
        }
    }
    public Action<Color> onUserColorChange;
    Color userColor = Color.clear;

    Logger logger;
    NetPlayer player;
    NetPlayerDecorator decorator;

    protected override void OnInitialize()
    {
        logger = FindAnyObjectByType<Logger>();

        NetworkManager.OnClientConnectedCallback += onClientConnect;        //클라이언트가 접속 할때마다 실행(서버에서는항상 실행, 클라이언트는 자기것만 실행)
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;    //클라이언트가 접속 해제할때마다 실행(서버에서는항상 실행, 클라이언트는 자기것만 실행)

        playersInGame.OnValueChanged += (_, newValue) => onPlayersInGameChange?.Invoke(newValue);
    }

    private void onClientConnect(ulong id)  //접속한 클라이언트의 id         //서버
    {
        NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);  //이 아이디를 가진 클라이언트에 플레이어 겜오브제 지급
        if(netObj.IsOwner) //서버장일때
        {
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player_{id}";

            decorator = netObj.GetComponent<NetPlayerDecorator>();
            decorator.SetName(UserName);

            foreach(var other in NetworkManager.SpawnManager.SpawnedObjectsList)        //섭장이 늦게 들갔을때
            {
                NetPlayer otherPlayer = other.GetComponent<NetPlayer>();
                if (otherPlayer != null && otherPlayer != player)
                {
                    otherPlayer.gameObject.name = $"OtherPlayer_{other.OwnerClientId}";
                }
            }
        }
        else 
        {
            NetPlayer other = netObj.GetComponent<NetPlayer>();
            if(other != null && other != player) 
            {
                netObj.gameObject.name = $"OtherPlayer_{id}";
            }
        }
        if (IsServer) 
        {
            playersInGame.Value++;
        }
    }
    private void OnClientDisconnect(ulong id)
    {
        NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);
        if(netObj.IsOwner) 
        {
            player = null;
        }

        if (IsServer)
        {
            playersInGame.Value--;
        }
    }

    public void Log(string msg) 
    {
        logger.Log(msg);
    }
}
