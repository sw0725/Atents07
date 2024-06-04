using StarterAssets;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform FireTransform => transform.GetChild(0);

    public Action<GunBase> onGunChange;

    StarterAssetsInputs starterAssets;
    FirstPersonController controller;
    GameObject GunCamera;
    GunBase[] guns;

    GunBase activeGun;
    GunBase defaultGun;

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<FirstPersonController>();
        GunCamera = transform.GetChild(2).gameObject;

        Transform c = transform.GetChild(3);
        guns = c.GetComponentsInChildren<GunBase>(true);
        defaultGun = guns[0];
    }

    private void Start()
    {
        starterAssets.onZoom += DisableGunCamera;

        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
        foreach (GunBase gun in guns)
        {
            gun.onFire += controller.FireRecoil;
            gun.onFire += (expand) => crosshair.Expend(expand * 10);
        }

        activeGun = defaultGun;
        activeGun.Equip();
        onGunChange?.Invoke(activeGun);
    }

    void DisableGunCamera(bool disable = true) 
    {
        GunCamera.SetActive(!disable);
    }

    public void GunChange(GunType gun) 
    {
        activeGun.gameObject.SetActive(false);
        activeGun.UnEquip();

        activeGun = guns[(int)gun];
        activeGun.Equip();
        activeGun.gameObject.SetActive(true);

        onGunChange?.Invoke(activeGun);
    }

    public void GunFire(bool isFireStart) 
    {
        activeGun.Fire(isFireStart);
    }

    public void GunReload() 
    {
        Revolver revolver = activeGun as Revolver;
        if (revolver != null) 
        {
            revolver.Reload();
        }
    }

    public void AddBulletCountChangeDelegate(Action<int> callback) //함수를 받아서 액션으로 등록해줌
    {
        foreach (GunBase gun in guns) 
        {
            gun.onBulletCountChange += callback;
        }
    }
}
