using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemDividerUI : MonoBehaviour
{
    //2개 이상 아이템이 들간 슬롯에 쉬프트 클릭시 열림
    //인풋필드, 플마버튼, 슬라이더로 갯수 설정
    //나누는 갯수는 반드시 1~슬롯의 아이템수-1
    //o버튼시 슬롯에서 설정갯수만큼 아이템 빠지고 빠진만큼 템프슬롯으로 x시 취소
    public Action<uint, uint> onOkClick;    //타겟슬롯 인덱스와 divideCount
    public Action onCancle;
    
    uint divideCount = MinItemCount;
    uint DivideCount 
    {
        get => divideCount;
        set 
        {
            divideCount = value;        //슬라이더와 글자수 바꾸기
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
