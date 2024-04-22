using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetSingltrun<GameManager>
{
    public NetPlayer Player => player;
    NetPlayer player;

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
    string userName = DefaultName;
    const string DefaultName = "Player";

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

    public NetPlayerDecorator Decorator => decorator;
    NetPlayerDecorator decorator;

    public Action onPlayerDisconnected;

    public CinemachineVirtualCamera VCam => virtualCamera;
    CinemachineVirtualCamera virtualCamera;

    Logger logger;

    protected override void OnInitialize()
    {
        logger = FindAnyObjectByType<Logger>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();

        NetworkManager.OnClientConnectedCallback += onClientConnect;        //Ŭ���̾�Ʈ�� ���� �Ҷ����� ����(�����������׻� ����, Ŭ���̾�Ʈ�� �ڱ�͸� ����)
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;    //Ŭ���̾�Ʈ�� ���� �����Ҷ����� ����(�����������׻� ����, Ŭ���̾�Ʈ�� �ڱ�͸� ����)

        playersInGame.OnValueChanged += (_, newValue) => onPlayersInGameChange?.Invoke(newValue);
    }

    private void onClientConnect(ulong id)  //������ Ŭ���̾�Ʈ�� id         //����
    {
        NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);  //�� ���̵� ���� Ŭ���̾�Ʈ�� �÷��̾� �׿����� ����
        if(netObj.IsOwner) //�������϶�
        {
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player_{id}";

            decorator = netObj.GetComponent<NetPlayerDecorator>();
            if(UserName != DefaultName) 
            {
                Decorator.SetName($"{UserName}_{id}");
                UserName = UserName;
            }
            else
            {
                Decorator.SetName($"{DefaultName}_{id}");
                UserName = UserName;
            }
            if(UserColor != Color.clear) decorator.SetColor(UserColor);

            foreach(var other in NetworkManager.SpawnManager.SpawnedObjectsList)        //������ �ʰ� �鰬����
            {
                NetPlayer otherPlayer = other.GetComponent<NetPlayer>();
                if (otherPlayer != null && otherPlayer != player)
                {
                    otherPlayer.gameObject.name = $"OtherPlayer_{other.OwnerClientId}";
                }
                NetPlayerDecorator netPlayerDecorator = other.GetComponent<NetPlayerDecorator>() ;
                if(netPlayerDecorator != null && netPlayerDecorator != Decorator) 
                {
                    netPlayerDecorator.RefreshNamePlate();
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
        //NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);
        //if(netObj.IsOwner)                                                                        //���ʰ� ������ ��쿡�� ��Ŀ��Ʈ���� �����°� �����Ƿ� ����ȵ�

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
