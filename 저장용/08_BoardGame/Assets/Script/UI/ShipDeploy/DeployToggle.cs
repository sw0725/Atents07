using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployToggle : MonoBehaviour
{
    public ShipType shipType = ShipType.None;
    public Action<DeployToggle> onSelect;

    UserPlayer player;
    Image image;
    GameObject deployEnd;

    enum DeployState : byte 
    {
        NotSelect = 0,
        Select,
        Deployed
    }

    DeployState state = DeployState.NotSelect;
    DeployState State 
    {
        get => state;
        set 
        {
            if(state != value) 
            {
                state = value;
                switch(state) 
                {
                    case DeployState.NotSelect:
                        image.color = Color.white;
                        deployEnd.SetActive(false);
                        player.UndoShipDeploy(shipType);
                        break;
                    case DeployState.Select:
                        image.color = selectColor;
                        deployEnd.SetActive(false);
                        player.SelectShipToDeploy(shipType);
                        onSelect?.Invoke(this);
                        break; 
                    case DeployState.Deployed:
                        image.color = selectColor;
                        deployEnd.SetActive(true);
                        break;
                }
            }
        }
    }

    readonly Color selectColor = new Color(1, 1, 1, 0.2f);

    private void Awake()
    {
        image = GetComponent<Image>();
        deployEnd = transform.GetChild(0).gameObject;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        player = GameManager.Instance.User;

        Ship targetShip = player.GetShip(shipType);
        if(targetShip != null) 
        {
            targetShip.onDeploy += (isDeploy) =>
            {
                if (isDeploy)
                {
                    State = DeployState.Deployed;
                }
                else
                {
                    State = DeployState.NotSelect;
                }
            };
        }
    }

    private void OnClick()
    {
        switch(state) 
        {
            case DeployState.NotSelect:
                State = DeployState.Select;
                //함선 배치모드로 전환
                break;
            case DeployState.Select:
                State = DeployState.NotSelect;
                break;
            case DeployState.Deployed:
                //배치된 배 취소
                State = DeployState.NotSelect;
                break;
        }
    }

    public void SetNotSelect() 
    {
        if(State != DeployState.Deployed) 
        {
            State = DeployState.NotSelect;
        }
    } 

#if UNITY_EDITOR
    public void Test_StateChange(int index) 
    {
        State = (DeployState)index;
    }
#endif
}
