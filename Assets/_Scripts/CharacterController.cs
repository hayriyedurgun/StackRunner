﻿using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
using Assets._Scripts.Utils;
using DG.Tweening;
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
        [SerializeField]
        private Rigidbody m_Rigidbody;

        private bool m_IsInitialized;

        private bool m_IsMovingForward;
        private bool m_IsMovingDestination;
        private Vector3 dest;
        private Tween m_DestinationTween;
        private Tween m_ForwardTween;

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

        private void Start()
        {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        private void Update()
        {
            if (m_IsInitialized &&
                (GameManager.Instance.GameState == GameState.PostGameOver ||
                GameManager.Instance.GameState == GameState.Playing) &&
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
                    while (GameManager.Instance.CurrentLevel.TileEdges.Any())
                    {
                        dest = GameManager.Instance.CurrentLevel.TileEdges.Dequeue();
                        if (transform.position.z < dest.z)
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

        private void MoveForward()
        {
            Debug.Log("Moving forward");

            m_IsMovingForward = true;
            var duration = 1 / Settings.CharacterSpeed;

            m_ForwardTween = DOTween.Sequence()
                            .Append(transform.DOMove(transform.forward, duration).SetRelative(true).SetEase(Ease.Linear))
                            .SetLoops(-1, LoopType.Incremental)
                            .OnKill(() =>
                            {
                                m_IsMovingForward = false;
                                Debug.Log("m_ForwardTween OnKill called!");
                            });
                            //.OnUpdate(() => Debug.Log("m_ForwardTween update called!"));
        }

        private void MoveDestination()
        {
            Debug.Log("Moving destination");

            m_IsMovingDestination = true;
            dest.y = 0;
            if (m_IsMovingForward)
            {
                m_ForwardTween?.Kill();
                m_IsMovingForward = false;
            }
            var duration = Vector3.Distance(transform.position, dest) / Settings.CharacterSpeed;

            m_DestinationTween = DOTween.Sequence()
                                .Append(transform.DOMove(dest, duration).SetEase(Ease.Linear))
                                //.Join(transform.DORotateQuaternion(Quaternion.LookRotation(dest - transform.position), Settings.CharacterSpeed).SetEase(Ease.Linear))
                                .OnKill(() => {
                                    m_IsMovingDestination = false;
                                    Debug.Log("m_DestinationTween OnKill called!");

                                });
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
                CameraManager.Instance.ChangeCam(CameraManager.Instance.WinCam, .75f);

                GameManager.Instance.TileContainer.SetCheckPoint(transform.position);

                m_DestinationTween?.Kill(true);
                m_ForwardTween?.Kill(true);
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
            else if (MovementState == MovementState.Dead)
            {
                Debug.Log("Dead!");

                m_DestinationTween?.Kill();
                m_ForwardTween?.Kill();

                m_Animator.SetTrigger(nameof(MovementState.Idle));

                GameManager.Instance.GameState = GameState.GameOver;
                CameraManager.Instance.VirtualCam.Follow = null;
                CameraManager.Instance.VirtualCam.LookAt = null;

                m_Rigidbody.isKinematic = false;
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
            m_Rigidbody.isKinematic = true;

            transform.position = GameManager.Instance.TileContainer.RecoverPosition();
            transform.rotation = Quaternion.Euler(Vector3.zero);
            m_IsMovingDestination = false;
            m_IsMovingForward = false;

            m_ForwardTween?.Kill();
            m_DestinationTween?.Kill();

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
