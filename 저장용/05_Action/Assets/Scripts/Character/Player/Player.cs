using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour, IHealth, IMana, IEquipTarget, IBattler
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
    public Action<float> onHealthCange { get; set; }        //프로퍼티 변수처럼 쓰기
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
            if (inventory.TempSlot != value) 
            {
                partsSlot[(int)part] = value;
            }
        }
    }      //part = 확인할 종류 에 해당하는 인트값을 가진 인덱스를반환
    public float AttackPower => attackPower;
    public float DefencePower => defencePower;
    public float baseAttackPower = 5.0f;
    public float baseDefencePower = 1.0f;

    float mp = 150.0f;
    float maxMP = 150.0f;
    float hp = 100.0f;
    float maxHP = 100.0f;
    float currentSpeed = 0.0f;
    float coolTime = 0.0f;
    float attackPower = 5.0f;
    float defencePower = 1.0f;
    float lockOnRange = 5.0f;

    int money = 0;

    Transform weaponParent;
    Transform shiledParent;
    PlayerInputController inputController;
    Animator animator;
    CharacterController characterController;
    Action<bool> onWeaponEffectEnable;
    Action<bool> onWeaponBladeEnable;
    Inventory inventory;
    InvenSlot[] partsSlot;                  //장비 아이템의 부위별 장비 상태(장착한 아이템이 있는 슬롯을 가짐, null일시 장비 없음)
    CinemachineVirtualCamera deadCam;
    PlayerSkillArea skillArea;
    Transform lockOnTarget;
    Transform LockOnTarget
    {
        get => lockOnTarget;
        set 
        {
            if (lockOnTarget != value) 
            {
                lockOnTarget = value;
                if(lockOnTarget != null) 
                {
                    lockOnEffect.SetParent(lockOnTarget);
                    lockOnEffect.localPosition = Vector3.zero;
                    lockOnEffect.gameObject.SetActive(true);

                    Enemy enemy = lockOnTarget.GetComponent<Enemy>();
                    enemy.onDie += () => LockOnTarget = null;
                }
                else 
                {
                    lockOnEffect.gameObject.SetActive(false);
                    lockOnEffect.SetParent(this.transform);
                    lockOnEffect.localPosition = Vector3.zero;
                }
            }
        }
    }
    Transform lockOnEffect;

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

    public Action<int> onHit { get; set; }

    Vector3 inputDir = Vector3.zero;    //점프없음 - y=0
    Quaternion targetRotation = Quaternion.identity;    //바라볼 방향

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

        lockOnEffect = transform.GetChild(3);

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>();
        inputController.onMove += OnMoveInput;
        inputController.onMoveModeChange += OnMoveModeChangeInput;
        inputController.onAttack += OnAttackInput;
        inputController.onItemPickUp += OnItemPickUpInput;
        inputController.onLockOn += OnLockOnInput;

        inputController.onSkillStart += OnSkillStart;
        inputController.onSkillEnd += OnSkillEnd;
                                    //이넘에서 지원하는 함수, 해당타입의 이넘을 넘긴다.
        partsSlot = new InvenSlot[Enum.GetValues(typeof(EquipType)).Length];
        deadCam = GetComponentInChildren<CinemachineVirtualCamera>();
        skillArea = GetComponentInChildren<PlayerSkillArea>(true);
    }

    private void Start()
    {
        attackPower = baseAttackPower;
        defencePower = baseDefencePower;
        inventory = new Inventory(this);        //만들어질때 게임메니져 필요해서 무조건 스타트부터
        if (GameManager.Instance.InventoryUI != null) 
        {
            GameManager.Instance.InventoryUI.InitializeInventory(Inventory);    //인벤토리와 ui연결
        }
        //ShowWeaponEffect(false);
    }

    private void Update()
    {
        coolTime -= Time.deltaTime;
        characterController.Move(Time.deltaTime * currentSpeed * inputDir);
        if(LockOnTarget != null) 
        {
            targetRotation = Quaternion.LookRotation(LockOnTarget.transform.position - transform.position);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
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

        if (isPress) //눌려짐
        {
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); //cam의 y축 회전 추출
            inputDir = camY * inputDir;                                     //입력 방향을 카메라의 y 회전과 같은 정도로 회전
            targetRotation = Quaternion.LookRotation(inputDir);         //몸체 회전

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
    {   //스피어 콜라이더를 잠깐 생성, 충돌하는 것을 반환. NonAlloc이 있는 함수는 배열을 생성하지 않는다. 원래있는 배열 재사용 가능
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
                conumable.Consume(this.gameObject);     //플레이어에게 즉시 사용
                item.End();
            }
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        deadCam.Follow = null;
        deadCam.Priority = 20;
        onDie?.Invoke();
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
                HP += Time.deltaTime * regenPerSec;     //초당 회족량만큼 증가
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

            switch (part) 
            {
                case EquipType.Weapon:
                    Weapon weapon = obj.GetComponentInChildren<Weapon>();
                    onWeaponEffectEnable = weapon.EffectEnable;
                    onWeaponBladeEnable = weapon.bladeColliderEnable;

                    ItemDataWeapon weaponData = equip as ItemDataWeapon;
                    attackPower = baseAttackPower + weaponData.attackPower;
                    break;
                case EquipType.Shield:
                    ItemDataShield shieldData = equip as ItemDataShield;
                    defencePower = baseDefencePower + shieldData.defencePower;
                    break;
            }
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

            switch (part)
            {
                case EquipType.Weapon:
                    onWeaponEffectEnable = null;
                    onWeaponBladeEnable = null;
                    attackPower = baseAttackPower;
                    break;
                case EquipType.Shield:
                    defencePower = baseDefencePower;
                    break;
            }
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

    void WeaponBladeEnable() 
    {
        onWeaponBladeEnable?.Invoke(true);
    }

    void WeaponBladeDisable()
    {
        onWeaponBladeEnable?.Invoke(false);
    }

    public void onItemEquip(InvenSlot slot) 
    {
        ItemDataEquip equip = slot.ItemData as ItemDataEquip;
        this[equip.EquipType] = slot;
    }

    public void Attack(IBattler target)
    {
        target.Defence(AttackPower);
    }

    public void Defence(float damage)
    {
        if (IsAlive) 
        {
            animator.SetTrigger("Hit");
            WeaponBladeDisable();                                 //공격중 맞았을때

            float final = MathF.Max(0, (damage - DefencePower)); //0이하로는 떨어지지 않게(회복되니까)
            HP -= final;
            onHit?.Invoke(Mathf.RoundToInt(final));
        }
    }

    private void OnLockOnInput()
    {
        Collider[] enemys = Physics.OverlapSphere(transform.position, lockOnRange, LayerMask.GetMask("AttackTarget"));

        Transform nearest = null;
        float nearestDistance = float.MaxValue;
        foreach (Collider enemy in enemys)
        {
            Vector3 dir = enemy.transform.position - transform.position;
            float distanseSqr = dir.sqrMagnitude;
            if (distanseSqr < nearestDistance)
            {
                nearest = enemy.transform;
                nearestDistance = distanseSqr;
            }
        }

        LockOnTarget = nearest;
    }

    private void OnSkillEnd()
    {
        skillArea.Deactivate();
    }

    private void OnSkillStart()
    {
        skillArea.Activate(AttackPower);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, ItemPickUpRange, 2.0f);
        
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, lockOnRange, 2.0f);
    }
#endif
}
