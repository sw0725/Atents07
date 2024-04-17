using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNetController : MonoBehaviour
{
    private void Start()
    {
        Transform c = transform.GetChild(0);
        Button startHosh = c.GetComponent<Button>();
        startHosh.onClick.AddListener(() => 
        {           //��Ʈ��ũ �Ŵ����� �̱�����, ��ŸƮȣ��Ʈ�� �� ������������ ȣ��Ʈ�� �̹� ���� ������ ���¶�� �Ǵٸ� ���� ����� �õ��� ���з� ������ �ϱ⶧��  
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("ȣ��Ʈ ����");
            }
            else 
            {
                Debug.Log("ȣ��Ʈ ����");
            }
        });

        c = transform.GetChild(1);
        Button startClient = c.GetComponent<Button>();
        startClient.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient()) //���� ��������� �ʾҰų� ��Ʈ��ũ ���ῡ ���н�
            {
                Debug.Log("Ŭ���̾�Ʈ ���� ����");
            }
            else 
            {
                Debug.Log("Ŭ���̾�Ʈ ���� ����");
            }
        });

        c = transform.GetChild (2);
        Button disconnect = c.GetComponent<Button>();
        disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();    //�ڽ��� ���� ����
        });
    }
}
