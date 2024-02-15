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
            canvasGroup.blocksRaycasts = true;          //클리어시 UI간섭 방지  **UI는 맨아래가 맨 위에 그려진다(우선권이 있다)
        };
    }
}
