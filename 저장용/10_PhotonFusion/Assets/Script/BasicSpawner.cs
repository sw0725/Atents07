using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;           //INetworkRunnerCallbacks사용위해 필요
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

    async void StartGame(GameMode gameMode)     //async = 비동기 실행 함수, 이 함수가 시작되면 끝날때까지 대기한다.(await)
    {                                           //세션을 열거나 게임에 접속한다 gameMode = 호스트?클라이언트?싱글?
        myRunner = this.gameObject.AddComponent<NetworkRunner>();
        myRunner.ProvideInput = true;             //유저 입력을 제공할 것이라 설정 => onInput 작동 가능

        SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);  //현재씬을 기반으로 씬 래퍼런스 가져옴
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        if(scene.IsValid) 
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await myRunner.StartGame(new StartGameArgs()      //러너의 스타트게임 실행후 대기
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

    void OnGUI()        //유니티제공, GUI그리는 이벤트 함수 코드대로 UI생성
    {
        if(myRunner == null) 
        {
            if(GUI.Button(new Rect(0,0,200,40), "Host")) //0,0좌표에 200*40크기의 host라 적힌 버튼을 생성
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Client"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)  //runner=자기자신의 러너(추정), player=접속자
    {
        if (runner.IsServer) 
        {
            Vector3 spawnpos = new Vector3(player.RawEncoded % runner.Config.Simulation.PlayerCount, 0, 0); //RawEncoded = 몇번째 플레이어인가 .인덱스 반환(-1=Host, 0~플레이어)
            NetworkObject netPlayer = runner.Spawn(playerPrefab, spawnpos, Quaternion.identity, player);    //위치를 구하고 넷워크 오브제에 접속자 연결(오브제에 입력을 줄 플레이어)
            spawnedCharacters.Add(player, netPlayer);                                                       //플레이어 전체확인 밎 접근용(명부)
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject)) 
        {
            runner.Despawn(networkObject);          //러너에서 디스폰(겜오브제삭제도 함께 처리)
            spawnedCharacters.Remove(player);       //명부 제거
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)       //사용자의 입력데이터 수집 input=데이터를 받아갈 변수
    {
        NetworkInputData data = new NetworkInputData();                 //이거 맴버변수로 빼도 작동됨 = 계속 new해줄 필요 없을지도
        data.direction = inputDir;                                      //if (Keyboard.current.wKey.isPressed) 이걸로 입력캐치가 되나 함수호출시마다 하드웨어에 접근해야함

        data.buttons.Set(NetworkInputData.MouseButtonLeft, isShootPress);

        input.Set(data);    //결정된 입력을 서버에 전달
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
