using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    public GameObject prefabBall;
    public GameObject prefabPhyscBall;
    public Material bodyMaterial;
    public float moveSpeed = 5.0f;

    [Networked]
    TickTimer delay { get; set; }
    [Networked]
    public bool spawnedProjectile { get; set; }

    Vector3 forward = Vector3.forward;

    NetworkCharacterController cc;
    ChangeDetector changeDetector;          //Networked로 설정된 변수의 변화를 감지하는 클래스
    PlayerInputAction actions;

    TMP_Text messageText;

    private void Awake()
    {
        cc = GetComponent<NetworkCharacterController>();
        Transform c = transform.GetChild(0);
        bodyMaterial = c.GetComponent<Renderer>()?.material;

        actions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Chat.performed += OnChat;
    }

    private void OnDisable()
    {
        actions.Player.Chat.performed -= OnChat;
        actions.Player.Disable();
    }

    public override void FixedUpdateNetwork()   //네트워크 틱별로 계속 실행되는 함수
    {
        if (GetInput(out NetworkInputData data)) 
        {
            //data.direction.Normalize();

            cc.Move(Runner.DeltaTime * moveSpeed * data.direction); //초당 무브스피드의 속도로 data의 디랙션 방향으로 이동
            
            if(data.direction.sqrMagnitude > 0) 
            {
                forward = data.direction;       //회전 도중 forward방향으로 공이 발사되는 것을 방지
            }

            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner)) //호스트인가, 딜레이가 미설정이거나 만료되었는가 
            {
                if (data.buttons.IsSet(NetworkInputData.MouseButtonLeft))   //마우스 왼쪽 버튼이 눌려져있다
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);  //발사 쿨타임 0.5초 세팅

                    //생성할 것, 생성 위치, 생성 각도, 생성물 입력권한, 스폰 직전에 실행되는 함수
                    Runner.Spawn(prefabBall, transform.position + forward, Quaternion.LookRotation(forward), Object.InputAuthority, (runner, obj) => { obj.GetComponent<Ball>().Init(); });

                    spawnedProjectile = !spawnedProjectile;
                }

                if (data.buttons.IsSet(NetworkInputData.MouseButtonRight))   //마우스 왼쪽 버튼이 눌려져있다
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);  //발사 쿨타임 0.5초 세팅

                    //생성할 것, 생성 위치, 생성 각도, 생성물 입력권한, 스폰 직전에 실행되는 함수
                    Runner.Spawn(prefabPhyscBall, transform.position + forward + Vector3.up, Quaternion.LookRotation(forward), Object.InputAuthority, (runner, obj) => { obj.GetComponent<PhyscBall>().Init(moveSpeed * forward); });

                    spawnedProjectile = !spawnedProjectile;
                }
            }
        }
    }

    public override void Spawned()      //스폰된 이후에 실행됨
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);  //시뮬레이션중인 상태에서(실시간) 변화가 있는가 감시
    }

    public override void Render()       //렌더링 종료후(시뮬레이션 종료후) 실행(업데이트와 유사)   //왠만하면 여기서 네트워크는 안쓰는게 좋다 -> 로컬로 동작하게
    {
        foreach(string chage in changeDetector.DetectChanges(this))     //이 네트워크 오브제의 Networked로 설정된 변수중 변화가 있던 것들을 모두 순회
        {
            switch(chage) 
            {
                case nameof(spawnedProjectile):         //spawnedProjectile변수 변경시
                    bodyMaterial.color = Color.white;
                    break;
            }
        }

        bodyMaterial.color = Color.Lerp(bodyMaterial.color, Color.blue, Time.deltaTime);    //render는 유니티 랜더 루프상에서 작동 = Update와 같은 간격
    }

    private void OnChat(InputAction.CallbackContext context)        //RPC발동용 입력처리
    {
        if (Object.HasInputAuthority)                               //ProvideInput(입력권한) 있는가 = 자기자신인가
        {
            RPC_SendMessage("Hello World");
        }
    }

    //이 함수를 쓰는자는(소스) 입력권한이 있어야하고 = 내 플레이어 // 이 함수를 받는자는(타겟) 상태권한이 있어야한다 = 호스트 // 플레이어 입장에서 RPC를 호출한다
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)] 
    public void RPC_SendMessage(string meg, RpcInfo info = default) //서버요청용 함수는 앞에 RPC붙여야함 info = 상태정보
    {
        RPC_RelayMessage(meg, info.Source); //info.Source = 로컬플레이어(자기 자신의 PlayerRef)
    }

    //소스는 호스트, 받은 내용은 모두에게 뿌림, 서버입장에서 RPC를 보낸다
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string meg, PlayerRef messageSource)
    {
        if (messageText == null) { messageText = FindAnyObjectByType<TMP_Text>(); }
        if (messageSource == Runner.LocalPlayer)
        {
            //서버가 내가 보낸 메세지를 받은경우
            meg = $"You : {meg}\n";
        }
        else 
        {
            //서버가 타인이 보낸 메세지를 받은경우
            meg = $"Other : {meg}\n";
        }
        messageText.text += meg;
    }
}
