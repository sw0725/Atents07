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
    ChangeDetector changeDetector;          //Networked�� ������ ������ ��ȭ�� �����ϴ� Ŭ����
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

    public override void FixedUpdateNetwork()   //��Ʈ��ũ ƽ���� ��� ����Ǵ� �Լ�
    {
        if (GetInput(out NetworkInputData data)) 
        {
            //data.direction.Normalize();

            cc.Move(Runner.DeltaTime * moveSpeed * data.direction); //�ʴ� ���꽺�ǵ��� �ӵ��� data�� �𷢼� �������� �̵�
            
            if(data.direction.sqrMagnitude > 0) 
            {
                forward = data.direction;       //ȸ�� ���� forward�������� ���� �߻�Ǵ� ���� ����
            }

            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner)) //ȣ��Ʈ�ΰ�, �����̰� �̼����̰ų� ����Ǿ��°� 
            {
                if (data.buttons.IsSet(NetworkInputData.MouseButtonLeft))   //���콺 ���� ��ư�� �������ִ�
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);  //�߻� ��Ÿ�� 0.5�� ����

                    //������ ��, ���� ��ġ, ���� ����, ������ �Է±���, ���� ������ ����Ǵ� �Լ�
                    Runner.Spawn(prefabBall, transform.position + forward, Quaternion.LookRotation(forward), Object.InputAuthority, (runner, obj) => { obj.GetComponent<Ball>().Init(); });

                    spawnedProjectile = !spawnedProjectile;
                }

                if (data.buttons.IsSet(NetworkInputData.MouseButtonRight))   //���콺 ���� ��ư�� �������ִ�
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);  //�߻� ��Ÿ�� 0.5�� ����

                    //������ ��, ���� ��ġ, ���� ����, ������ �Է±���, ���� ������ ����Ǵ� �Լ�
                    Runner.Spawn(prefabPhyscBall, transform.position + forward + Vector3.up, Quaternion.LookRotation(forward), Object.InputAuthority, (runner, obj) => { obj.GetComponent<PhyscBall>().Init(moveSpeed * forward); });

                    spawnedProjectile = !spawnedProjectile;
                }
            }
        }
    }

    public override void Spawned()      //������ ���Ŀ� �����
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);  //�ùķ��̼����� ���¿���(�ǽð�) ��ȭ�� �ִ°� ����
    }

    public override void Render()       //������ ������(�ùķ��̼� ������) ����(������Ʈ�� ����)   //�ظ��ϸ� ���⼭ ��Ʈ��ũ�� �Ⱦ��°� ���� -> ���÷� �����ϰ�
    {
        foreach(string chage in changeDetector.DetectChanges(this))     //�� ��Ʈ��ũ �������� Networked�� ������ ������ ��ȭ�� �ִ� �͵��� ��� ��ȸ
        {
            switch(chage) 
            {
                case nameof(spawnedProjectile):         //spawnedProjectile���� �����
                    bodyMaterial.color = Color.white;
                    break;
            }
        }

        bodyMaterial.color = Color.Lerp(bodyMaterial.color, Color.blue, Time.deltaTime);    //render�� ����Ƽ ���� �����󿡼� �۵� = Update�� ���� ����
    }

    private void OnChat(InputAction.CallbackContext context)        //RPC�ߵ��� �Է�ó��
    {
        if (Object.HasInputAuthority)                               //ProvideInput(�Է±���) �ִ°� = �ڱ��ڽ��ΰ�
        {
            RPC_SendMessage("Hello World");
        }
    }

    //�� �Լ��� �����ڴ�(�ҽ�) �Է±����� �־���ϰ� = �� �÷��̾� // �� �Լ��� �޴��ڴ�(Ÿ��) ���±����� �־���Ѵ� = ȣ��Ʈ // �÷��̾� ���忡�� RPC�� ȣ���Ѵ�
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)] 
    public void RPC_SendMessage(string meg, RpcInfo info = default) //������û�� �Լ��� �տ� RPC�ٿ����� info = ��������
    {
        RPC_RelayMessage(meg, info.Source); //info.Source = �����÷��̾�(�ڱ� �ڽ��� PlayerRef)
    }

    //�ҽ��� ȣ��Ʈ, ���� ������ ��ο��� �Ѹ�, �������忡�� RPC�� ������
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string meg, PlayerRef messageSource)
    {
        if (messageText == null) { messageText = FindAnyObjectByType<TMP_Text>(); }
        if (messageSource == Runner.LocalPlayer)
        {
            //������ ���� ���� �޼����� �������
            meg = $"You : {meg}\n";
        }
        else 
        {
            //������ Ÿ���� ���� �޼����� �������
            meg = $"Other : {meg}\n";
        }
        messageText.text += meg;
    }
}
