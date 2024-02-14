using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
                                                //클릭감지
public class VirtualButton : MonoBehaviour, IPointerClickHandler
{
    public Action onJumpInput;

    Image image;

    private void Awake()
    {
        Transform c = transform.GetChild(1);
        image = c.GetComponent<Image>();
        image.fillAmount = 0.0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onJumpInput?.Invoke();
    }

    public void RefreashCoolTime(float a) 
    {
        image.fillAmount = a;
    }

    public void Stop() 
    {
        onJumpInput = null;
        image.fillAmount = 1.0f;
    }
}
