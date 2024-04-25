using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

public class NetEnergyOrb : NetworkBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 20.0f;
    public float expolsionRadius = 5.0f;

    Rigidbody rb;
    VisualEffect effect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        effect = GetComponent<VisualEffect>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer && IsOwner) 
        {
            transform.Rotate(-30.0f, 0, 0);
            rb.velocity = speed * transform.forward;
            StartCoroutine(selfDespawn());
        }
    }

    IEnumerator selfDespawn() 
    {
        yield return new WaitForSeconds(lifeTime);
        if(IsOwner && this.NetworkObject.IsSpawned)
        {
            if (IsServer)
            {
                this.NetworkObject.Despawn();
            }
            else
            {
                DespawnRequestServerRpc();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)                                //�浹������ ���������� �����ϴ�
    {
        if (!IsOwner && !this.NetworkObject.IsSpawned) return;                        //�������� �浹�� �����Ѵ�.

        Collider[] result = Physics.OverlapSphere(transform.position, expolsionRadius, LayerMask.GetMask("Player"));

        if (result.Length > 0)
        {
            List<ulong> targets = new List<ulong>(result.Length);
            foreach (Collider col in result)
            {
                NetPlayer netPlayer = col.gameObject.GetComponent<NetPlayer>();
                targets.Add(netPlayer.OwnerClientId);
            }

            ClientRpcParams clientRpcParams = new ClientRpcParams 
            {
                Send = new ClientRpcSendParams 
                {
                    TargetClientIds = targets.ToArray(),
                }
            };
            PlayerDieRequestClientRpc(clientRpcParams);
        }

        EffectProcessRequestClientRpc();
    }

    IEnumerator EffectFinishProcess() 
    {
        int Size_ID = Shader.PropertyToID("Size");                          //����Ʈ�� ������Ƽ ��������
        int EffectFinishEvent_ID = Shader.PropertyToID("OnEffectFinish");
        
        float timer = 0.0f;
        float expendDuration = 0.5f;                                        //0-1���̰��� ������� -> �帥�ð�(timer)/�� �Ⱓ(expendDuration)
        float preCompute = (1 / expendDuration) * expolsionRadius;          //�� �����⸦ ���ϰ��� expendDuration�� �̸� 1/expendDuration ������ ������ �ΰ� ����
        while (timer < expendDuration) 
        {
            timer += Time.deltaTime;
            effect.SetFloat(Size_ID, timer * preCompute);                   //������Ƽ �� ����
            yield return null;                                              //�̰� �־�� 1�����Ӵ� while���� �ѹ� ���°� ����
        }                                                                   //���ϸ� while ���� ���İ��� ����Ǽ� update(�����Ӵ� �ѹ�ȣ��) ȿ���� ������

        float reductionDuration = 1.0f;
        timer = reductionDuration;
        float preCom = 1 / reductionDuration * expolsionRadius;
        while (timer > 0) 
        {
            timer -= Time.deltaTime;
            effect.SetFloat (Size_ID, timer * preCom);
            yield return null;
        }
        effect.SendEvent(EffectFinishEvent_ID);                             //�̺�Ʈ ������

        while (effect.aliveParticleCount > 0)                               //����Ʈ�� ��ƼŬ ������ 0�� �Ǹ� 
        {
            yield return null;
        }

        if (IsServer)
        {
            NetworkObject.Despawn();
        }
        else 
        {
            if(IsOwner)
                DespawnRequestServerRpc();
        }
    }

    [ServerRpc]                                                             //Ŭ���̾�Ʈ�� �������� ���� �Ƿ�
    void DespawnRequestServerRpc()                                          //ServerRpc�� �� �Լ� �̸��� ServerRpc�� �ٿ�����
    {
        this.NetworkObject.Despawn();
    }

    [ServerRpc]                                                             //Ŭ���̾�Ʈ�� �������� ���� �Ƿ�
    void VelocityRequestServerRpc(Vector3 newValue)
    {
        rb.velocity = newValue;
    }

    [ClientRpc]                                                             //������ ��� Ŭ���̾�Ʈ���� �� �� �Ƿ�
    void EffectProcessRequestClientRpc()
    {
        rb.useGravity = false;
        rb.drag = Mathf.Infinity;

        StartCoroutine(EffectFinishProcess());
    }

    [ClientRpc]                                                             //������ ��� Ŭ���̾�Ʈ���� �� �� �Ƿ�
    void PlayerDieRequestClientRpc(ClientRpcParams clientRpcParams = default)   //����Ʈ = ��ο���
    {
        NetPlayer player = GameManager.Instance.Player;
        player.SendChat($"{GameManager.Instance.Player.name}");
    }
}

