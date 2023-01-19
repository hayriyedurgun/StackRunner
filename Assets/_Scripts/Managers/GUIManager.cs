using Assets._Scripts.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets._Scripts.Managers
{
    public class GUIManager : MonoBehaviour
    {
        [Inject]
        private GameController m_GameController;

        public LoadingPanel LoadingPanel;
        public PlayingPanel PlayingPanel;
        public SuccessPanel SuccessPanel;
        public GameOverPanel GameOverPanel;

        private void Start()
        {
            m_GameController.GameStateChanged += OnGameStateChanged;
            OnGameStateChanged(m_GameController.GameState);
        }

        private void OnDestroy()
        {
            if (m_GameController)
            {
                m_GameController.GameStateChanged -= OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.PostGameOver) return;

            LoadingPanel.ChangeVisibility(state == GameState.Loading);
            PlayingPanel.ChangeVisibility(state == GameState.Playing);
            SuccessPanel.ChangeVisibility(state == GameState.Success);
            GameOverPanel.ChangeVisibility(state == GameState.GameOver);
        }
    }
}
