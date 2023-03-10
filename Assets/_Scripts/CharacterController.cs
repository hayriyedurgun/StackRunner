using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
using Assets._Scripts.Utils;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Scripts
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody m_RigidBody;
        [SerializeField]
        private Animator m_Animator;
        [SerializeField]
        private Rigidbody m_Rigidbody;
        [SerializeField]
        private float Speed = 2;

        [Inject]
        private CameraController m_Camera;

        [Inject]
        private GameController m_GameController;

        private bool m_IsInitialized;

        private bool m_IsMovingForward;
        private bool m_IsMovingDestination;
        private Vector3 m_Dest;
        private Tween m_DestinationTween;
        private Tween m_ForwardTween;

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

        private void Start()
        {
            m_GameController.GameStateChanged += OnGameStateChanged;
        }

        private void Update()
        {
            if (m_IsInitialized &&
                (m_GameController.GameState == GameState.PostGameOver ||
                m_GameController.GameState == GameState.Playing) &&
                MovementState != MovementState.Dead)
            {
                //Detect grounded
                var ray = new Ray(transform.position + Vector3.up * .5f, Vector3.down);
                var rayCast = Physics.SphereCast(ray, .25f, 1, LayerHelper.Or(Layer.Tile));
                if (!rayCast)
                {
                    MovementState = MovementState.Dead;
                    m_IsInitialized = false;
                    return;
                }

                if (!m_IsMovingDestination)
                {
                    while (m_GameController.CurrentLevel.TileEdges.Any())
                    {
                        m_Dest = m_GameController.CurrentLevel.TileEdges.Dequeue();
                        if (transform.position.z < m_Dest.z)
                        {
                            MoveDestination();
                            break;
                        }
                    }

                    if (!m_IsMovingForward && !m_IsMovingDestination)
                    {
                        MoveForward();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (m_GameController)
            {
                m_GameController.GameStateChanged -= OnGameStateChanged;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == (int)Layer.Finish)
            {
                other.enabled = false;
                m_GameController.GameState = GameState.Success;
                MovementState = MovementState.Dancing;

                m_Camera.WinCam.transform.LookAt(transform.position);
                m_Camera.ChangeCam(m_Camera.WinCam, 1f);

                m_GameController.TileContainer.SetCheckPoint(transform.position);

                m_DestinationTween?.Kill();
                m_ForwardTween?.Kill();
            }
            else if (other.gameObject.layer == (int)Layer.Collectible)
            {
                var collectible = other.GetComponent<CollectibleBehaviour>();

                collectible.Collect();
            }
        }

        private void MoveForward()
        {
            m_IsMovingForward = true;
            var duration = 1 / Speed;

            m_ForwardTween = DOTween.Sequence()
                            .Append(transform.DOMove(Vector3.forward * 8, duration * 8).SetRelative(true).SetEase(Ease.Linear))
                            .Join(transform.DORotateQuaternion(Quaternion.LookRotation(Vector3.forward, Vector3.up), Mathf.Min(.1f, duration)))                            
                            .OnKill(() =>
                            {
                                m_IsMovingForward = false;
                            });
        }

        private void MoveDestination()
        {
            m_IsMovingDestination = true;
            m_Dest.y = 0;
            if (m_IsMovingForward)
            {
                m_ForwardTween?.Kill();
                m_IsMovingForward = false;
            }
            var duration = Vector3.Distance(transform.position, m_Dest) / Speed;
            var direction = m_Dest - transform.position;

            m_DestinationTween = DOTween.Sequence()
                                .Append(transform.DOMove(m_Dest, duration).SetEase(Ease.Linear))
                                .Join(transform.DORotateQuaternion(Quaternion.LookRotation(direction, Vector3.up), Mathf.Min(.1f, duration)))
                                .OnKill(() =>
                                {
                                    m_IsMovingDestination = false;
                                });
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
            else if (MovementState == MovementState.Dead)
            {
                m_DestinationTween?.Kill();
                m_ForwardTween?.Kill();

                m_Animator.SetTrigger(nameof(MovementState.Idle));

                m_GameController.GameState = GameState.GameOver;

                m_Camera.VirtualCam.Follow = null;
                m_Camera.VirtualCam.LookAt = null;

                m_Rigidbody.isKinematic = false;
            }
        }

        private void OnGameStateChanged(GameState gamestate)
        {
            if (gamestate == GameState.Playing)
            {
                m_GameController.GameStateChanged -= OnGameStateChanged;
                MovementState = MovementState.Running;
            }
        }

        public void OnLevelStarted()
        {
            //transform.position = Vector3.zero;
            if (m_GameController.GameState != GameState.Loading)
            {
                MovementState = MovementState.Running;
            }
            else
            {
                MovementState = MovementState.Idle;
            }

            m_Camera.VirtualCam.LookAt = transform;
            m_Camera.VirtualCam.Follow = transform;
            m_Rigidbody.isKinematic = true;

            transform.position = m_GameController.TileContainer.RecoverPosition();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            m_IsMovingDestination = false;
            m_IsMovingForward = false;

            m_ForwardTween?.Kill();
            m_DestinationTween?.Kill();

            m_Camera.ChangeCam(m_Camera.VirtualCam, 0f);
            m_IsInitialized = true;
        }
    }

    public enum MovementState
    {
        Idle,
        Running,
        Dancing,
        Dead
    }
}
