using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.Instance.onClear += () =>
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        };
    }
}
