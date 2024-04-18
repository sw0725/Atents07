using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetSingltrun<GameManager>
{
    public NetPlayer Player => player;

    Logger logger;
    NetPlayer player;

    protected override void OnInitialize()
    {
        logger = FindAnyObjectByType<Logger>();

        NetworkManager.OnClientConnectedCallback += onClientConnect;        //Ŭ���̾�Ʈ�� ���� �Ҷ����� ����(�����������׻� ����, Ŭ���̾�Ʈ�� �ڱ�͸� ����)
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;    //Ŭ���̾�Ʈ�� ���� �����Ҷ����� ����(�����������׻� ����, Ŭ���̾�Ʈ�� �ڱ�͸� ����)
    }

    private void onClientConnect(ulong id)  //������ Ŭ���̾�Ʈ�� id         //����
    {
        NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);  //�� ���̵� ���� Ŭ���̾�Ʈ�� �÷��̾� �׿����� ����
        if(netObj.IsOwner) //�������϶�
        {
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player_{id}";

            foreach(var other in NetworkManager.SpawnManager.SpawnedObjectsList)        //������ �ʰ� �鰬����
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
    }
    private void OnClientDisconnect(ulong id)
    {
        NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);
        if(netObj.IsOwner) 
        {
            player = null;
        }
    }

    public void Log(string msg) 
    {
        logger.Log(msg);
    }
}
