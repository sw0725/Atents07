using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public Action<Tab> onTabSelected;

    public bool IsSelected 
    {
        get => isSelected;
        set 
        {
            isSelected = value;
            if (isSelected) 
            {
                tabImage.color = SelectedColor;
                SubPanelOpen();
                onTabSelected?.Invoke(this);
            }
            else 
            {
                tabImage.color = UnSelectedColor;    
                SubPanelClose();
            }
        }
    }
    bool isSelected = false;

    Image tabImage;
    TabSubPanel subPanel;

    readonly Color UnSelectedColor = new Color(0.4f, 0.4f, 0.4f, 1f);
    readonly Color SelectedColor = Color.white;

    private void Awake()
    {
        tabImage = GetComponent<Image>();
        subPanel = GetComponentInChildren<TabSubPanel>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => IsSelected = true );
    }

    public void SubPanelOpen() 
    {
        subPanel.gameObject.SetActive( true );
    }

    public void SubPanelClose()
    {
        subPanel.gameObject.SetActive(false);
    }
}
