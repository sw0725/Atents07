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
    public class StartMenu : MonoBehaviour//메뉴 처리용유틸리티 클래스
    {
        [SerializeField] private NetworkRunner _networkRunnerPrefab = null; //네트워크 러너 프리펩
        [SerializeField] private PlayerData _playerDataPrefab = null;       //플레이어 데이터프리펩(이름관련)

        [SerializeField] private TMP_InputField _nickName = null;           //플레이어 이름용 인풋필드

        // The Placeholder Text is not accessible through the TMP_InputField component so need a direct reference
        [SerializeField] private TextMeshProUGUI _nickNamePlaceholder = null;//인풋필드의 디폴트 텍스트(랜덤이름 설정용)

        [SerializeField] private TMP_InputField _roomName = null;           //방이름용
        [SerializeField] private string _gameSceneName = null;              //게임씬으로 넘어가기위해 필요

        private NetworkRunner _runnerInstance = null;                       //네트워크 러너

        // Attempts to start a new game session 
        public void StartHost()         //호스트로 세션을 시작하는 함수
        {
            SetPlayerData();
            StartGame(GameMode.AutoHostOrClient, _roomName.text, _gameSceneName);
        }

        public void StartClient()       //클라이언트로 세션을 시작하는 함수
        {
            SetPlayerData();
            StartGame(GameMode.Client, _roomName.text, _gameSceneName);
        }

        private void SetPlayerData()    //플레이어 데이터 설정
        {
            var playerData = FindObjectOfType<PlayerData>();    //왜찾는지 모르게씀
            if (playerData == null)
            {
                playerData = Instantiate(_playerDataPrefab);    //데이터가 없으면새로 생성
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