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
    public Action<float> onHPChange;       //���� hp
    public Action<float> onAttacked;       //���ݹ��� ����(���� 0, �ĸ� 180)
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
                if (!(activeGun is Revolver))   //a is b a�� bŸ���϶� true
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

    public void AddBulletCountChangeDelegate(Action<int> callback) //�Լ��� �޾Ƽ� �׼����� �������
    {
        foreach (GunBase gun in guns) 
        {
            gun.onBulletCountChange += callback;
        }
    }

    public void OnAttacked(Enemy enemy) 
    {
        Vector3 dir = enemy.transform.position - transform.position;
                                                                                  //Vector3.up������ ������ // transform.up ������ ������
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);    //angle = 0-180������ ������ ��ȯ, ������ ���� ���� �Ұ� //SignedAngle = 0-360 ��ȯ ����� ���� ȸ���� ����
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
        playerInput.actions.actionMaps[0].Disable();        //�׼Ǹ��� �ϳ��� �����Ƿ� �׳� ó��
    }
}
