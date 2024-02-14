using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.onOver += OverPanel;
        gameObject.SetActive(false);
    }

    void OverPanel()
    {
        gameObject.SetActive(true);
    }
}
