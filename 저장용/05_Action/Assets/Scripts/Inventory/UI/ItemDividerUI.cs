using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemDividerUI : MonoBehaviour
{
    //2�� �̻� �������� �鰣 ���Կ� ����Ʈ Ŭ���� ����
    //��ǲ�ʵ�, �ø���ư, �����̴��� ���� ����
    //������ ������ �ݵ�� 1~������ �����ۼ�-1
    //o��ư�� ���Կ��� ����������ŭ ������ ������ ������ŭ ������������ x�� ���
    public Action<uint, uint> onOkClick;    //Ÿ�ٽ��� �ε����� divideCount
    public Action onCancle;
    
    uint divideCount = MinItemCount;
    uint DivideCount 
    {
        get => divideCount;
        set 
        {
            divideCount = value;        //�����̴��� ���ڼ� �ٲٱ�
        }
    } 

    Image image;
    TMP_InputField inputField;
    Slider slider;

    PlayerInputAction inputAction;
    InvenSlot targetSlot;

    const int MinItemCount = 1;

    private void Awake()
    {
        Transform c = transform.GetChild(0);

        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.Click.performed += OnClick;
    }

    private void OnDisable()
    {
        inputAction.UI.Click.performed -= OnClick;
        inputAction.UI.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        
    }

    public void Open(InvenSlot target) 
    {
    
    }

    public void Close() 
    {
    
    }
}
