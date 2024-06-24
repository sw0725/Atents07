using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;
using System.ComponentModel.Design;

namespace Asteroids.HostSimple
{
    // A utility class which defines the behaviour of the various buttons and input fields found in the Menu scene
    public class StartMenu : MonoBehaviour//�޴� ó������ƿ��Ƽ Ŭ����
    {
        [SerializeField] private NetworkRunner _networkRunnerPrefab = null; //��Ʈ��ũ ���� ������
        [SerializeField] private PlayerData _playerDataPrefab = null;       //�÷��̾� ������������(�̸�����)

        [SerializeField] private TMP_InputField _nickName = null;           //�÷��̾� �̸��� ��ǲ�ʵ�

        // The Placeholder Text is not accessible through the TMP_InputField component so need a direct reference
        [SerializeField] private TextMeshProUGUI _nickNamePlaceholder = null;//��ǲ�ʵ��� ����Ʈ �ؽ�Ʈ(�����̸� ������)

        [SerializeField] private TMP_InputField _roomName = null;           //���̸���
        [SerializeField] private string _gameSceneName = null;              //���Ӿ����� �Ѿ������ �ʿ�

        private NetworkRunner _runnerInstance = null;                       //��Ʈ��ũ ����

        // Attempts to start a new game session 
        public void StartHost()         //ȣ��Ʈ�� ������ �����ϴ� �Լ�
        {
            SetPlayerData();
            StartGame(GameMode.AutoHostOrClient, _roomName.text, _gameSceneName);
        }

        public void StartClient()       //Ŭ���̾�Ʈ�� ������ �����ϴ� �Լ�
        {
            SetPlayerData();
            StartGame(GameMode.Client, _roomName.text, _gameSceneName);
        }

        private void SetPlayerData()    //�÷��̾� ������ ����
        {
            var playerData = FindObjectOfType<PlayerData>();    //��ã���� �𸣰Ծ�
            if (playerData == null)
            {
                playerData = Instantiate(_playerDataPrefab);    //�����Ͱ� ��������� ����
            }

            if (string.IsNullOrWhiteSpace(_nickName.text))
            {
                playerData.SetNickName(_nickNamePlaceholder.text);
            }
            else
            {
                playerData.SetNickName(_nickName.text);
            }
        }

        private async void StartGame(GameMode mode, string roomName, string sceneName)
        {
            _runnerInstance = FindObjectOfType<NetworkRunner>();
            if (_runnerInstance == null)
            {
                _runnerInstance = Instantiate(_networkRunnerPrefab);
            }

            // Let the Fusion Runner know that we will be providing user input
            _runnerInstance.ProvideInput = true;

            var startGameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName,
                ObjectProvider = _runnerInstance.GetComponent<NetworkObjectPoolDefault>(),
            };

            // GameMode.Host = Start a session with a specific name
            // GameMode.Client = Join a session with a specific name
            await _runnerInstance.StartGame(startGameArgs);

            if (_runnerInstance.IsServer)
            {
                _runnerInstance.LoadScene(sceneName);
            }
        }
    }
}