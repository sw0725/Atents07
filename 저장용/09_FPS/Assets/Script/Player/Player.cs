using StarterAssets;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform FireTransform => transform.GetChild(0);

    StarterAssetsInputs starterAssets;
    FirstPersonController controller;
    GameObject GunCamera;
    GunBase[] guns;

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<FirstPersonController>();
        GunCamera = transform.GetChild(2).gameObject;

        Transform c = transform.GetChild(3);
        guns = c.GetComponentsInChildren<GunBase>();
        foreach (GunBase gun in guns) 
        {
            gun.onFire += controller.FireRecoil;
        }
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
