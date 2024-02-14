using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.onClear += ClearPanel;
        gameObject.SetActive(false);
    }

    void ClearPanel() 
    {
        gameObject.SetActive(true);
    }
}
