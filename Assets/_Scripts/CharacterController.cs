using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody m_RigidBody;
        [SerializeField]
        private Animator m_Animator;
        private bool m_IsInitialized;

        public GameplaySettings Settings => GameManager.Instance.GameplaySettings;

        private MovementState m_MovementState;
        public MovementState MovementState
        {
            get => m_MovementState;
            private set
            {
                if (m_MovementState != value)
                {
                    m_MovementState = value;
                    OnMovementStateChanged();
                }
            }
        }

        public Transform Destination
        {
            get
            {
                if (GameManager.Instance.CurrentLevel.CurrentTile)
                {
                    return GameManager.Instance.CurrentLevel.CurrentTile.transform;
                }
                else
                {
                    return transform;
                }
            }
        }

        private void Start()
        {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        private void Update()
        {
            if (m_IsInitialized &&
                transform.position.y < -0.1f &&
                (GameManager.Instance.GameState == GameState.PostGameOver ||
                GameManager.Instance.GameState == GameState.Playing ))
            {
                GameManager.Instance.GameState = GameState.GameOver;
                CameraManager.Instance.VirtualCam.Follow = null;
                CameraManager.Instance.VirtualCam.LookAt = null;

                m_IsInitialized = false;
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState == GameState.Playing || GameManager.Instance.GameState == GameState.PostGameOver)
            {
                var dir = ((Destination.position + transform.forward * 5) - transform.position).normalized;
                m_RigidBody.MovePosition(transform.position + dir * Time.fixedDeltaTime * Settings.CharacterSpeed);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.GameStateChanged -= OnGameStateChanged;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == (int)Layer.Finish)
            {
                other.isTrigger = false;
                GameManager.Instance.GameState = GameState.Success;
                MovementState = MovementState.Dancing;

                CameraManager.Instance.WinCam.transform.LookAt(transform.position);
                CameraManager.Instance.ChangeCam(CameraManager.Instance.WinCam, .1f);

                GameManager.Instance.TileContainer.SetCheckPoint(transform.position);
            }
        }

        private void OnMovementStateChanged()
        {
            if (MovementState == MovementState.Dancing)
            {
                m_Animator.SetTrigger(nameof(MovementState.Dancing));
            }
            else if (MovementState == MovementState.Running)
            {
                m_Animator.SetTrigger(nameof(MovementState.Running));
            }
        }

        private void OnGameStateChanged(GameState gamestate)
        {
            if (gamestate == GameState.Playing)
            {
                GameManager.Instance.GameStateChanged -= OnGameStateChanged;
                MovementState = MovementState.Running;
            }
        }

        public void OnLevelStarted()
        {
            //transform.position = Vector3.zero;
            if (GameManager.Instance.GameState != GameState.Loading)
            {
                MovementState = MovementState.Running;
            }
            else
            {
                MovementState = MovementState.Idle;
            }

            CameraManager.Instance.VirtualCam.LookAt = transform;
            CameraManager.Instance.VirtualCam.Follow = transform;

            transform.position = GameManager.Instance.TileContainer.RecoverPosition();
            m_IsInitialized = true;
        }
    }

    public enum MovementState
    {
        Idle,
        Running,
        Dancing
    }
}
