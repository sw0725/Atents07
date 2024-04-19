using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerDecorator : NetworkBehaviour
{
    NetworkVariable<Color> bodyColor = new NetworkVariable<Color>();
    NetworkVariable<FixedString32Bytes> userName = new NetworkVariable<FixedString32Bytes>();

    Renderer playerRenderer;
    Material bodyMaterial;
    NamePlate namePlate;

    readonly int BaseColor_Hash = Shader.PropertyToID("_BaceColor");

    private void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>();
        bodyMaterial = playerRenderer.material;

        bodyColor.OnValueChanged += OnBodyColorChange;
        namePlate = GetComponentInChildren<NamePlate>();
        userName.OnValueChanged += OnNameSet;
    }

    private void OnBodyColorChange(Color previousValue, Color newValue)
    {
        bodyMaterial.SetColor(BaseColor_Hash, newValue);
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer) 
        {
            bodyColor.Value = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);    //색상, 채도, 명도를 랜덤으로 지정해 색을 만든다. => 색상만 랜덤돌리도록
        }
        bodyMaterial.SetColor(BaseColor_Hash, bodyColor.Value);
    }

    public void SetName(string name) 
    {
        if (IsOwner)
        {
            if (IsServer)
            {
                userName.Value = name;
            }
            else 
            {
                UserNameRequestServerRpc(name);
            }
        }
    }

    private void OnNameSet(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        namePlate.SetName(newValue.ToString());
    }
    public void RefreshNamePlate(string name) 
    {
        namePlate.SetName(name.ToString());
    }

    [ServerRpc]
    void UserNameRequestServerRpc(string name) 
    {
        userName.Value = name;
    }
}
