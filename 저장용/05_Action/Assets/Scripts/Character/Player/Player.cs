using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour, IHealth, IMana, IEquipTarget
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;
    public float turnSpeed = 10.0f;
    [Range(0, AttackAnimationLength)]
    public float maxCoolTime = 0.3f;
    public float ItemPickUpRange = 2.0f;
    public Action<int> onMoneyChange;
    public Inventory Inventory => inventory;
    public int Money 
    {
        get => money;
        set 
        {
            if (money != value) 
            {
                money = value;
                onMoneyChange?.Invoke(money);
            }
        }
    }
    public float HP 
    {
        get => hp;
        set 
        {
            if (IsAlive) 
            {
                hp = value;
                if (hp <= 0.0f) 
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0, MaxHP);
                onHealthCange?.Invoke(hp/MaxHP);
            }
        }
    }
    public float MaxHP => maxHP;
    public Action<float> onHealthCange { get; set; }        //������Ƽ ����ó�� ����
    public bool IsAlive => hp > 0;
    public Action onDie { get; set; }
    public float MP 
    {
        get => mp;
        set
        {
            if (IsAlive)
            {
                mp = Mathf.Clamp(value, 0, MaxMP);
                onManaCange?.Invoke(mp / MaxMP);
            }
        }
    }
    public float MaxMP => maxMP;
    public Action<float> onManaCange { get; set; }
    public InvenSlot this[EquipType part] 
    {
        get => partsSlot[(int)part];
        set 
        {
            partsSlot[(int)part] = value;
        }
    }      //part = Ȯ���� ���� �� �ش��ϴ� ��Ʈ���� ���� �ε�������ȯ

    float mp = 150.0f;
    float maxMP = 150.0f;
    float hp = 100.0f;
    float maxHP = 100.0f;
    float currentSpeed = 0.0f;
    float coolTime = 0.0f;

    int money = 0;

    Transform weaponParent;
    Transform shiledParent;
    PlayerInputController inputController;
    Animator animator;
    CharacterController characterController;
    Action<bool> onWeaponEffectEnable;
    Inventory inventory;
    InvenSlot[] partsSlot;                  //��� �������� ������ ��� ����(������ �������� �ִ� ������ ����, null�Ͻ� ��� ����)

    readonly int speed_Hash = Animator.StringToHash("Speed");
    readonly int attack_Hash = Animator.StringToHash("Attack");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;
    const float AttackAnimationLength = 0.533f;

    enum MoveMode
    {
        Walk = 0,
        Run
    }
    MoveMode currentSpeedMode = MoveMode.Run;
    MoveMode CurrentSpeedMode
    {
        get => currentSpeedMode;
        set
        {
            currentSpeedMode = value;
            if (currentSpeed > 0.0f)
            {
                MoveSpeedChange(currentSpeedMode);
            }
        }
    }

    Vector3 inputDir = Vector3.zero;    //�������� - y=0
    Quaternion targetRotation = Quaternion.identity;    //�ٶ� ����

    private void Awake()
    {
        Transform c = transform.GetChild(2);
        c = c.GetChild(0);
        c = c.GetChild(0);
        c = c.GetChild(0);
        
        Transform spine3 = c.GetChild(0);
        weaponParent = spine3.GetChild(2);
        weaponParent = weaponParent.GetChild(1);
        weaponParent = weaponParent.GetChild(0);
        weaponParent = weaponParent.GetChild(0);
        weaponParent = weaponParent.GetChild(2);
        
        shiledParent = spine3.GetChild(1);
        shiledParent = shiledParent.GetChild(1);
        shiledParent = shiledParent.GetChild(0);
        shiledParent = shiledParent.GetChild(0);
        shiledParent = shiledParent.GetChild(2);

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>();
        inputController.onMove += OnMoveInput;
        inputController.onMoveModeChange += OnMoveModeChangeInput;
        inputController.onAttack += OnAttackInput;
        inputController.onItemPickUp += OnItemPickUpInput;
                                    //�̳ѿ��� �����ϴ� �Լ�, �ش�Ÿ���� �̳��� �ѱ��.
        partsSlot = new InvenSlot[Enum.GetValues(typeof(EquipType)).Length];
    }

    private void Start()
    {
        inventory = new Inventory(this);        //��������� ���Ӹ޴��� �ʿ��ؼ� ������ ��ŸƮ����
        if (GameManager.Instance.InventoryUI != null) 
        {
            GameManager.Instance.InventoryUI.InitializeInventory(Inventory);    //�κ��丮�� ui����
        }

        Weapon weapon = weaponParent.GetComponentInChildren<Weapon>();
        if (weapon != null) 
        {
            onWeaponEffectEnable += weapon.EffectEnable;
            ShowWeaponEffect(false);
        }
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        coolTime -= Time.deltaTime;
    }

    private void OnAttackInput()
    {
        if(coolTime < 0 && ((currentSpeed < 0.001f)||(currentSpeedMode == MoveMode.Walk)))
        {
            animator.SetTrigger(attack_Hash);
            coolTime = maxCoolTime;
        }
    }

    private void OnMoveModeChangeInput()
    {
        if (CurrentSpeedMode == MoveMode.Walk)
        {
            CurrentSpeedMode = MoveMode.Run;
        }
        else
        {
            CurrentSpeedMode = MoveMode.Walk;
        }
    }

    private void OnMoveInput(Vector2 input, bool isPress)
    {
        inputDir.x = input.x;
        inputDir.y = 0;
        inputDir.z = input.y;

        if (isPress) //������
        {
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); //cam�� y�� ȸ�� ����
            inputDir = camY * inputDir;                                     //�Է� ������ ī�޶��� y ȸ���� ���� ������ ȸ��
            targetRotation = Quaternion.LookRotation(inputDir);         //��ü ȸ��

            MoveSpeedChange(CurrentSpeedMode);
        }
        else
        {
            currentSpeed = 0;
            animator.SetFloat(speed_Hash, AnimatorStopSpeed);
        }
    }

    void MoveSpeedChange(MoveMode mode)
    {
        switch (mode)
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                animator.SetFloat(speed_Hash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                animator.SetFloat(speed_Hash, AnimatorRunSpeed);
                break;
        }
    }

    public void ShowWeaponAndShield(bool isShow = true) 
    {
        weaponParent.gameObject.SetActive(isShow);
        shiledParent.gameObject.SetActive(isShow);
    }

    public void ShowWeaponEffect(bool isShow = true) 
    {
        onWeaponEffectEnable?.Invoke(isShow);
    }

    private void OnItemPickUpInput()
    {   //���Ǿ� �ݶ��̴��� ��� ����, �浹�ϴ� ���� ��ȯ. NonAlloc�� �ִ� �Լ��� �迭�� �������� �ʴ´�. �����ִ� �迭 ���� ����
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, ItemPickUpRange, LayerMask.GetMask("Item"));
        foreach (Collider collider in itemColliders) 
        {
            ItemObject item = collider.GetComponent<ItemObject>();
            IConumable conumable = item.ItemData as IConumable;
            if (conumable == null)
            {
                if (Inventory.AddItem(item.ItemData.code))
                {
                    item.End();
                }
            }
            else 
            {
                conumable.Consume(this.gameObject);     //�÷��̾�� ��� ���
                item.End();
            }
        }
    }

    public void Die()
    {
        onDie?.Invoke();
        Debug.Log("Die");
    }

    public void HealthRegernerate(float totalRegen, float duration)
    {
        StartCoroutine(RegenCoroutine(totalRegen, duration, true));
    }

    public void HealthRegernerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        StartCoroutine(RegenByTick(tickRegen, tickInterval, totalTickCount, true));
    }

    IEnumerator RegenCoroutine(float totalRegen, float duration, bool isHP) 
    {
        float regenPerSec = totalRegen / duration;
        float timer = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (isHP)
            {
                HP += Time.deltaTime * regenPerSec;     //�ʴ� ȸ������ŭ ����
            }
            else
            {
                MP += Time.deltaTime * regenPerSec;
            }
            yield return null;
        }
    }
    IEnumerator RegenByTick(float tickRegen, float tickInterval, uint totalTickCount, bool isHP)
    {
        WaitForSeconds wait = new WaitForSeconds(tickInterval);
        for (int i = 0; i < totalTickCount; i++)
        {
            if (isHP)
            {
                HP += tickRegen;
            }
            else 
            {
                MP += tickRegen;
            }
            yield return wait;
        }
    }

    public void ManaRegernerate(float totalRegen, float duration)
    {
        StartCoroutine(RegenCoroutine(totalRegen, duration, false));
    }

    public void ManaRegernerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        StartCoroutine(RegenByTick(tickRegen, tickInterval, totalTickCount, false));
    }

    public void EquipItem(EquipType part, InvenSlot slot)
    {
        ItemDataEquip equip = slot.ItemData as ItemDataEquip;
        if (equip != null) 
        {
            Transform partParent = GetEquipParentTransform(part);
            GameObject obj = Instantiate(equip.epuipPrefab, partParent);
            this[part] = slot;
            slot.IsEquipped = true;
        }
    }

    public void UnEquipItem(EquipType part)
    {
        InvenSlot slot = partsSlot[(int)part];
        if (slot != null) 
        {
            Transform partParent = GetEquipParentTransform(part);
            while (partParent.childCount > 0) 
            {
                Transform c = partParent.GetChild(0);
                c.SetParent(null);
                Destroy(c.gameObject);
            }
            slot.IsEquipped = false;
            this[part] = null;
        }
    }

    public Transform GetEquipParentTransform(EquipType part)
    {
        Transform result = null;
        switch(part) 
        {
            case EquipType.Weapon:
                result = weaponParent;
                break;
            case EquipType.Shield:
                result = shiledParent;
                break;
        }
        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, ItemPickUpRange, 2.0f);
    }
#endif
}
