using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;                               //�����ٿ��� �ݵ�� ���� �����
                                                           //IDrag ���� //IPointer ���콺���� UP�� ���콺���� �ٿ��� ������
public class VirtualStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler  
{       //Canvas���� UI�� ����ϴ� Transform           //IEndDragHandler, IBeginDragHandler�� �巡���ڵ鷯�� �ʼ�
    RectTransform handleRect;
    RectTransform containerRect;

    float stickRange;

    public Action<Vector2> onMoveInput;

    private void Awake()
    {
        containerRect = GetComponent<RectTransform>();
        Transform c = transform.GetChild(0);
        handleRect = c.GetComponent<RectTransform>();

        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }                               //rect=ȣ���� �������� �����ϴ� �簢�� ����

    public void OnDrag(PointerEventData eventData)  //�Ͼ�� ���� ������ �Ӽ����� ǥ��
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect,                  //�̿�������
            eventData.position,             //�� ��ũ����ǥ��
            eventData.pressEventCamera,     //�� ī�޶� ��������
            out Vector2 position            //���÷� �󸶳� ������ ���ΰ� ����
            );
        position = Vector2.ClampMagnitude(position, stickRange);

        InputUpdate(position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {       //if(eventData.button = PointerEventData.InputButton.Left) //�� ���� ������ ���� ����
        InputUpdate(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    void InputUpdate(Vector2 inputDelta) 
    {                   //��Ŀ���� �Ÿ� //position �ǹ��� ��ġ
        handleRect.anchoredPosition = inputDelta;
        onMoveInput?.Invoke(inputDelta/stickRange); //(1,1~-1,-1�� ����ȭ)
    }

    public void Stop()
    {
        onMoveInput = null;
    }
}
