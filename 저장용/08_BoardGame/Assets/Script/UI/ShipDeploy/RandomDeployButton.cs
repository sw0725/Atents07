using UnityEngine;
using UnityEngine.UI;

public class RandomDeployButton : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        UserPlayer user = GameManager.Instance.User;
        if (user.IsAllDeployed) 
        {
            user.UndoAllShipDeployment();
        }
        user.AutoShipDeployment(true);
    }

}
