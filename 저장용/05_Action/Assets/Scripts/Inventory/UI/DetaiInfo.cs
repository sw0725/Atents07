using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DetaiInfo : MonoBehaviour
{
    Image itemIcon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    TextMeshProUGUI itemDescription;

    CanvasGroup canvasGroup;

    public float alphaChangeSpeed = 10.0f;
    public bool IsPause 
    {
        get => isPause;
        set 
        {
            isPause = value;
            if (isPause) 
            {
                Close();
            }
        }
    }
    
    bool isPause = false;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        itemIcon = c.GetComponent<Image>();
        c = transform.GetChild(1);
        itemName = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(2);
        itemPrice = c.GetComponent<TextMeshProUGUI>();
        c = transform.GetChild(4);
        itemDescription = c.GetComponent<TextMeshProUGUI>();

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    public void Open(ItemData itemData) 
    {
        if (!IsPause && itemData != null)
        {
            itemIcon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;          //ToString �������� N0 = 3�ڸ����� ,
            itemPrice.text = itemData.price.ToString("N0");
            itemDescription.text = itemData.itemDescription;

            canvasGroup.alpha = 0.001f;                 //���������� ���Ƕ�����
            MovePosition(Mouse.current.position.ReadValue());

            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    public void Close() 
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    public void MovePosition(Vector2 screenPos) 
    {
        if (canvasGroup.alpha > 0.0f) 
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;    //�󸶳� ��ġ�°�
            screenPos.x -= Mathf.Max(0, over); // + ����(��ĥ����) �����ϵ���
            rect.position = screenPos;
        }
    }

    IEnumerator FadeIn() 
    {
        while (canvasGroup.alpha < 1.0f) 
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
    }

    IEnumerator FadeOut() 
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha += -Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
    }
}
