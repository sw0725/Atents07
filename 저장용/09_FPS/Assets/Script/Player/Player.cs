using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    StarterAssetsInputs starterAssets;
    GameObject GunCamera;

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        GunCamera = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        starterAssets.onZoom += DisableGunCamera;
    }

    void DisableGunCamera(bool disable = true) 
    {
        GunCamera.SetActive(!disable);
    }
}
