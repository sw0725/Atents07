using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;           //INetworkRunnerCallbacks������� �ʿ�
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner myRunner = null;

    [SerializeField]
    private NetworkPrefabRef playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    Vector3 inputDir = Vector3.zero;

    bool isShootPress = false;    
    
    PlayerInputAction inputActions;

    void Awake() 
    {
        inputActions = new PlayerInputAction();
    }

    async void StartGame(GameMode gameMode)     //async = �񵿱� ���� �Լ�, �� �Լ��� ���۵Ǹ� ���������� ����Ѵ�.(await)
    {                                           //������ ���ų� ���ӿ� �����Ѵ� gameMode = ȣ��Ʈ?Ŭ���̾�Ʈ?�̱�?
        myRunner = this.gameObject.AddComponent<NetworkRunner>();
        myRunner.ProvideInput = true;             //���� �Է��� ������ ���̶� ���� => onInput �۵� ����

        SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);  //������� ������� �� ���۷��� ������
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        if(scene.IsValid) 
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await myRunner.StartGame(new StartGameArgs()      //������ ��ŸƮ���� ������ ���
        {
            GameMode = gameMode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        InputEnable();
    }

    void InputEnable() 
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Shoot.performed += OnShootPress;
        inputActions.Player.Shoot.canceled += OnShootRelease;
    }


    void InPutDisable() 
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Shoot.performed -= OnShootPress;
        inputActions.Player.Shoot.canceled -= OnShootRelease;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 read = context.ReadValue<Vector2>();
        inputDir.Set(read.x, 0, read.y);
    }

    private void OnShootPress(InputAction.CallbackContext context)
    {
        isShootPress = true;
    }

    private void OnShootRelease(InputAction.CallbackContext context)
    {
        isShootPress = false;
    }

    void OnGUI()        //����Ƽ����, GUI�׸��� �̺�Ʈ �Լ� �ڵ��� UI����
    {
        if(myRunner == null) 
        {
            if(GUI.Button(new Rect(0,0,200,40), "Host")) //0,0��ǥ�� 200*40ũ���� host�� ���� ��ư�� ����
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Client"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)  //runner=�ڱ��ڽ��� ����(����), player=������
    {
        if (runner.IsServer) 
        {
            Vector3 spawnpos = new Vector3(player.RawEncoded % runner.Config.Simulation.PlayerCount, 0, 0); //RawEncoded = ���° �÷��̾��ΰ� .�ε��� ��ȯ(-1=Host, 0~�÷��̾�)
            NetworkObject netPlayer = runner.Spawn(playerPrefab, spawnpos, Quaternion.identity, player);    //��ġ�� ���ϰ� �ݿ�ũ �������� ������ ����(�������� �Է��� �� �÷��̾�)
            spawnedCharacters.Add(player, netPlayer);                                                       //�÷��̾� ��üȮ�� �G ���ٿ�(���)
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject)) 
        {
            runner.Despawn(networkObject);          //���ʿ��� ����(�׿����������� �Բ� ó��)
            spawnedCharacters.Remove(player);       //��� ����
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)       //������� �Էµ����� ���� input=�����͸� �޾ư� ����
    {
        NetworkInputData data = new NetworkInputData();                 //�̰� �ɹ������� ���� �۵��� = ��� new���� �ʿ� ��������
        data.direction = inputDir;                                      //if (Keyboard.current.wKey.isPressed) �̰ɷ� �Է�ĳġ�� �ǳ� �Լ�ȣ��ø��� �ϵ��� �����ؾ���

        data.buttons.Set(NetworkInputData.MouseButtonLeft, isShootPress);

        input.Set(data);    //������ �Է��� ������ ����
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }


    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
