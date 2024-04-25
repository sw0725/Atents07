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

    private void OnCollisionEnter(Collision collision)                                //충돌감지는 서버에서만 가능하다
    {
        if (!IsOwner && !this.NetworkObject.IsSpawned) return;                        //스폰전에 충돌시 무시한다.

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
        int Size_ID = Shader.PropertyToID("Size");                          //이펙트의 프로퍼티 가져오기
        int EffectFinishEvent_ID = Shader.PropertyToID("OnEffectFinish");
        
        float timer = 0.0f;
        float expendDuration = 0.5f;                                        //0-1사이값을 만드려면 -> 흐른시간(timer)/총 기간(expendDuration)
        float preCompute = (1 / expendDuration) * expolsionRadius;          //을 나누기를 피하고자 expendDuration를 미리 1/expendDuration 형으로 나누어 두고 곱셈
        while (timer < expendDuration) 
        {
            timer += Time.deltaTime;
            effect.SetFloat(Size_ID, timer * preCompute);                   //프로퍼티 값 수정
            yield return null;                                              //이게 있어야 1프레임당 while문을 한번 도는게 가능
        }                                                                   //안하면 while 문이 순식간에 종료되서 update(프레임당 한번호출) 효과를 못본다

        float reductionDuration = 1.0f;
        timer = reductionDuration;
        float preCom = 1 / reductionDuration * expolsionRadius;
        while (timer > 0) 
        {
            timer -= Time.deltaTime;
            effect.SetFloat (Size_ID, timer * preCom);
            yield return null;
        }
        effect.SendEvent(EffectFinishEvent_ID);                             //이벤트 보내기

        while (effect.aliveParticleCount > 0)                               //이펙트의 파티클 갯수가 0이 되면 
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

    [ServerRpc]                                                             //클라이언트가 서버에게 할일 의뢰
    void DespawnRequestServerRpc()                                          //ServerRpc는 꼭 함수 이름에 ServerRpc를 붙여야함
    {
        this.NetworkObject.Despawn();
    }

    [ServerRpc]                                                             //클라이언트가 서버에게 할일 의뢰
    void VelocityRequestServerRpc(Vector3 newValue)
    {
        rb.velocity = newValue;
    }

    [ClientRpc]                                                             //서버가 모든 클라이언트에게 할 일 의뢰
    void EffectProcessRequestClientRpc()
    {
        rb.useGravity = false;
        rb.drag = Mathf.Infinity;

        StartCoroutine(EffectFinishProcess());
    }

    [ClientRpc]                                                             //서버가 모든 클라이언트에게 할 일 의뢰
    void PlayerDieRequestClientRpc(ClientRpcParams clientRpcParams = default)   //디폴트 = 모두에게
    {
        NetPlayer player = GameManager.Instance.Player;
        player.SendChat($"{GameManager.Instance.Player.name}");
    }
}

