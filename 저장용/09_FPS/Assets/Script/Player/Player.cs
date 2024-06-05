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

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<FirstPersonController>();
        GunCamera = transform.GetChild(2).gameObject;

        Transform c = transform.GetChild(3);
        guns = c.GetComponentsInChildren<GunBase>(true);
    }

    private void Start()
    {
        starterAssets.onZoom += DisableGunCamera;

        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
        foreach (GunBase gun in guns)
        {
            gun.onFire += controller.FireRecoil;
            gun.onFire += (expand) => crosshair.Expend(expand * 10);
            gun.onAmmoDepleted += () => GunChange(GunType.Revolver);
        }

        activeGun = guns[0];
        activeGun.Equip();
        onGunChange?.Invoke(activeGun);
    }

    void DisableGunCamera(bool disable = true) 
    {
        GunCamera.SetActive(!disable);
    }

    public void GunChange(GunType gun) 
    {
        activeGun.UnEquip();
        activeGun.gameObject.SetActive(false);

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

    public void AddBulletCountChangeDelegate(Action<int> callback) //�Լ��� �޾Ƽ� �׼����� �������
    {
        foreach (GunBase gun in guns) 
        {
            gun.onBulletCountChange += callback;
        }
    }
}
