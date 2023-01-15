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

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState != GameState.Playing) return;
            m_RigidBody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * Settings.CharacterSpeed);

            //TODO: ortala!
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.GameStateChanged -= OnGameStateChanged;
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
