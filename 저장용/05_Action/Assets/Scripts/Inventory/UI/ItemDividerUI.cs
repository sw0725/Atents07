using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemDividerUI : MonoBehaviour
{
    public Action<uint, uint> onOkClick;    //Ÿ�ٽ��� �ε����� divideCount
    public Action onCancle;

    uint divideCount = MinItemCount;
    uint DivideCount 
    {
        get => divideCount;
        set 
        {
            divideCount = Math.Clamp(value, MinItemCount, MaxItemCount);
            inputField.text = divideCount.ToString();
            slider.value = divideCount;
        }
    } 
    uint MaxItemCount => targetSlot.IsEmpty ? MinItemCount : (targetSlot.ItemCount-1);

    Image image;
    TMP_InputField inputField;
    Slider slider;

    PlayerInputAction inputAction;
    InvenSlot targetSlot;

    const uint MinItemCount = 1;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        image = c.GetComponent<Image>();

        c = transform.GetChild(1);
        inputField = c.GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener((text) => 
        {
            if (uint.TryParse(text, out uint value))
            {
                DivideCount = value;
            }
            else 
            {
                DivideCount = MinItemCount;     //������ �ް� �����Ǿ��־ �������� ���������� -�� �Ÿ����
            }
        });

        c = transform.GetChild(2);
        Button PluseButton = c.GetComponent<Button>();
        PluseButton.onClick.AddListener(() => 
        {
            DivideCount++;
        });

        c = transform.GetChild(3);
        Button MinusButton = c.GetComponent<Button>();
        MinusButton.onClick.AddListener(() =>
        {
            DivideCount--;
        });

        c = transform.GetChild(4);
        slider = c.GetComponent<Slider>();
        slider.onValueChanged.AddListener((value) => 
        {
            DivideCount = (uint)value;
        });

        c = transform.GetChild(5);
        Button OkButton = c.GetComponent<Button>();
        OkButton.onClick.AddListener(() =>
        {
            onOkClick?.Invoke(targetSlot.Index, DivideCount);
            Close();
        });

        c = transform.GetChild(6);
        Button CancleButton = c.GetComponent<Button>();
        CancleButton.onClick.AddListener(() =>
        {
            onCancle?.Invoke();
            Close();
        });

        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.Click.performed += OnClick;
        inputAction.UI.Wheel.performed += OnWheel;
    }

    private void OnDisable()
    {
        inputAction.UI.Click.performed -= OnClick;
        inputAction.UI.Wheel.performed -= OnWheel;
        inputAction.UI.Disable();
    }

    public bool Open(InvenSlot target) 
    {
        bool result = false;
        if (!target.IsEmpty && target.ItemCount > MinItemCount) 
        {
            targetSlot = target;
            image.sprite = target.ItemData.itemIcon;
            slider.minValue = MinItemCount;
            slider.maxValue = MaxItemCount;                 //�����̴��� �ִ��ּҰ� ����
            DivideCount = targetSlot.ItemCount / 2;

            result = true;
            gameObject.SetActive(true);
        }
        return result;
    }

    public void Close() 
    {
        gameObject.SetActive(false);
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (!MousePointInRect()) 
        {
            Close();
        }
    }

    private void OnWheel(InputAction.CallbackContext context)
    {
        if (MousePointInRect()) 
        {
            if (context.ReadValue<float>() > 0)
            {
                DivideCount++;
            }
            else 
            {
                DivideCount--;
            }
        }
    }

    bool MousePointInRect()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 diff = screenPos - (Vector2)transform.position;       // UI�� �Ǻ����� ���콺 �����Ͱ� �󸶳� �������°�
        RectTransform rect = (RectTransform)transform;
        return rect.rect.Contains(diff);                              //UI���� ���ΰ�
    }
}
