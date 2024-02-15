using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.Instance.onOver += () =>
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;          //Ŭ����� UI���� ����  **UI�� �ǾƷ��� �� ���� �׷�����(�켱���� �ִ�)
        };
    }
}
