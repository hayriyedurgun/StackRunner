﻿using Assets._Scripts.Managers;
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
            //Destination = transform.forward;
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
        }

        private void Update()
        {
            if (transform.position.y < -0.1f)
            {
                GameManager.Instance.GameState = GameState.GameOver;
                CameraManager.Instance.VirtualCam.Follow = null;
                CameraManager.Instance.VirtualCam.LookAt = null;
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
                GameManager.Instance.GameState = GameState.Success;
                MovementState = MovementState.Dancing;

                CameraManager.Instance.WinCam.transform.LookAt(transform.position);
                CameraManager.Instance.ChangeCam(CameraManager.Instance.WinCam, .1f);
            }
        }

        private void OnMovementStateChanged()
        {
            if (MovementState == MovementState.Dancing)
            {
                m_Animator.SetBool(nameof(MovementState.Dancing), true);
            }
            else if (MovementState == MovementState.Running)
            {
                m_Animator.SetBool(nameof(MovementState.Running), true);
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
    }

    public enum MovementState
    {
        Idle,
        Running,
        Dancing
    }
}
