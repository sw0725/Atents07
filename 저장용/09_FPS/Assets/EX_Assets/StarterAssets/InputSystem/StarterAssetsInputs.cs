using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public Action<bool> onZoom;

		bool isGameEnd = false;

		Player player;
		CinemachineVirtualCamera followCamera;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Start()
        {
			followCamera = GameManager.Instance.FollowCamera;
			GameManager.Instance.onGameClear += (_) => isGameEnd = true;
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputAction.CallbackContext context)
		{
			MoveInput(context.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			if (cursorInputForLook) 
			{
				LookInput(context.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			JumpInput(context.performed);
        }

		public void OnSprint(InputAction.CallbackContext context)
		{
			SprintInput(context.ReadValue<float>() > 0.1f);
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
			bool isPress = !context.canceled;

            StopAllCoroutines();
			StartCoroutine(Zoom(isPress));
			onZoom?.Invoke(isPress);
        }

		IEnumerator Zoom(bool zoomIn)
        {
            const float zoomFOV = 20.0f;
            const float normalFOV = 40.0f;
            const float zoomTime = 0.25f;
			float speed = (normalFOV - zoomFOV) / zoomTime;

            float fov = followCamera.m_Lens.FieldOfView;

			if (zoomIn)
            {
                while (fov > zoomFOV)
                {
                    fov -= Time.deltaTime * speed;
                    followCamera.m_Lens.FieldOfView = fov;
                    yield return null;
                }
                followCamera.m_Lens.FieldOfView = zoomFOV;
            }
			else
            {
                while (fov < normalFOV)
                {
                    fov += Time.deltaTime * speed;
                    followCamera.m_Lens.FieldOfView = fov;
                    yield return null;
                }
                followCamera.m_Lens.FieldOfView = normalFOV;
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
			if (context.performed) 
			{
				player.GunFire(true);
			}
			else if (context.canceled) 
			{
				player.GunFire(false);
			}	
        }

        public void OnReload(InputAction.CallbackContext context)
        {
			if (context.performed) 
			{
				player.GunReload();
			}
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			if (!isGameEnd) 
			{
				SetCursorState(cursorLocked);
			}
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}