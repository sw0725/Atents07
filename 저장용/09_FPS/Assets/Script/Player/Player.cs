using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MaxHP = 100.0f;
    public float HP 
    {
        get => hp;
        set 
        {
            hp = value;
            if (hp < 0.1f) 
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHPChange?.Invoke(hp);
        }
    }
    float hp;

    public Transform FireTransform => transform.GetChild(0);

    public Action<GunBase> onGunChange;
    public Action onDie;
    public Action<float> onHPChange;       //현재 hp
    public Action<float> onAttacked;       //공격받은 각도(정면 0, 후면 180)
    public Action onSpawn;

    StarterAssetsInputs starterAssets;
    FirstPersonController controller;
    GameObject GunCamera;
    GunBase[] guns;

    GunBase activeGun;
    PlayerInput playerInput;

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<FirstPersonController>();
        playerInput = GetComponent<PlayerInput>();

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
            gun.onAmmoDepleted += () => 
            {
                if (!(activeGun is Revolver))   //a is b a가 b타입일때 true
                {
                    GunChange(GunType.Revolver);
                }       
            };
        }

        activeGun = guns[0];
        activeGun.Equip();
        onGunChange?.Invoke(activeGun);

        HP = MaxHP;

        GameManager.Instance.onGameClear += (_) => InputDisable();
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

    public void AddBulletCountChangeDelegate(Action<int> callback) //함수를 받아서 액션으로 등록해줌
    {
        foreach (GunBase gun in guns) 
        {
            gun.onBulletCountChange += callback;
        }
    }

    public void OnAttacked(Enemy enemy) 
    {
        Vector3 dir = enemy.transform.position - transform.position;
                                                                                  //Vector3.up월드의 업방향 // transform.up 로컬의 업방향
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);    //angle = 0-180사이의 각도만 반환, 오른쪽 왼쪽 구분 불가 //SignedAngle = 0-360 반환 축기준 으로 회전각 나옴
        onAttacked?.Invoke(-angle);
        HP -= enemy.attackPower;
    }

    void Die() 
    {
        onDie?.Invoke();
        gameObject.SetActive(false);
    }

    public void Spawn() 
    {
        GameManager gameManager = GameManager.Instance;
        Vector3 centerPos = MazeVisualizer.GridToWorld(gameManager.MazeWidth / 2, gameManager.MazeHeight / 2);
        transform.position = centerPos;
        onSpawn?.Invoke();
    }

    void InputDisable() 
    {
        playerInput.actions.actionMaps[0].Disable();        //액션맵이 하나만 있으므로 그냥 처리
    }
}
