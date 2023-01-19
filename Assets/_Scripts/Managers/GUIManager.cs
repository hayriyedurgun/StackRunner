using Assets._Scripts.UI;
using TMPro;
using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class GUIManager : MonoBehaviour
    {
        public LoadingPanel LoadingPanel;
        public PlayingPanel PlayingPanel;
        public SuccessPanel SuccessPanel;
        public GameOverPanel GameOverPanel;


        private void Start()
        {
            GameManager.Instance.GameStateChanged += OnGameStateChanged;
            OnGameStateChanged(GameManager.Instance.GameState);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.GameStateChanged -= OnGameStateChanged;
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
