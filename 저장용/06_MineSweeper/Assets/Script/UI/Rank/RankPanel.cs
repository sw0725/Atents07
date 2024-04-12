using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    ToggleButton toggle;

    Tab[] tabs;
    Tab selectedTab;

    Tab SelectedTab 
    {
        get => selectedTab;
        set 
        {
            if(selectedTab != value) 
            {
                selectedTab.IsSelected = false;
                selectedTab = value;
                selectedTab.IsSelected = true;
            }
        }
    }

    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();
        foreach(Tab tab in tabs) 
        {
            tab.onTabSelected += (newSelected) => 
            {
                SelectedTab = newSelected;
                toggle.ToggleOn();
            };
        }
        toggle = GetComponentInChildren<ToggleButton>();
        toggle.onToggleChange += (isOn) => 
        {
            if (isOn)
            {
                SelectedTab.SubPanelOpen();
            }
            else 
            {
                SelectedTab.SubPanelClose();
            }
        };

        selectedTab = tabs[tabs.Length -1];     //���õ����� ������ ����
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.onGameClear += Open;
        gameManager.onGameReady += Close;

        Close();
    }

    void Open() //���� Ŭ����� ����
    {
        SelectedTab = tabs[0];
        gameObject.SetActive(true);
    }

    void Close()    //���� ����� �׻� ����
    {
        gameObject.SetActive(false);
    }
}
