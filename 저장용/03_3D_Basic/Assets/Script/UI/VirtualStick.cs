using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;                               //업과다운은 반드시 같이 써야함
                                                           //IDrag 관련 //IPointer 마우스관련 UP은 마우스땔때 다운은 누를때
public class VirtualStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler  
{       //Canvas상의 UI가 사용하는 Transform           //IEndDragHandler, IBeginDragHandler는 드래그핸들러가 필수
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
    }                               //rect=호출한 오브제가 차지하는 사각형 정보

    public void OnDrag(PointerEventData eventData)  //일어나는 일의 정보를 속성으로 표시
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect,                  //이영역에서
            eventData.position,             //이 스크린좌표가
            eventData.pressEventCamera,     //이 카메라 기준으로
            out Vector2 position            //로컬로 얼마나 움직일 것인가 리턴
            );
        position = Vector2.ClampMagnitude(position, stickRange);

        InputUpdate(position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {       //if(eventData.button = PointerEventData.InputButton.Left) //로 왼쪽 오른쪽 구분 가능
        InputUpdate(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    void InputUpdate(Vector2 inputDelta) 
    {                   //앵커기준 거리 //position 피벗의 위치
        handleRect.anchoredPosition = inputDelta;
        onMoveInput?.Invoke(inputDelta/stickRange); //(1,1~-1,-1로 정규화)
    }

    public void Stop()
    {
        onMoveInput = null;
    }
}
