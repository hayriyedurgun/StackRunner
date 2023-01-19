using Assets._Scripts.Factories;
using Assets._Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Scripts.Managers
{
    public class GameController : MonoBehaviour
    {
        private int m_CurrentLevelIndex;

        [Inject]
        private LevelFactory m_LevelFactory;

        [NonSerialized]
        public Level CurrentLevel;

        public TileContainer TileContainer;

        public event Action<GameState> GameStateChanged;

        public GameplaySettings GameplaySettings;

        public List<Level> Levels;

        public TileSplitObjectPool ObjectPool;

        private GameState m_GameState;
        public GameState GameState
        {
            get => m_GameState;
            set
            {
                if (m_GameState != value)
                {
                    m_GameState = value;
                    GameStateChanged?.Invoke(GameState);
                }
            }
        }

        private void Awake()
        {
            m_CurrentLevelIndex = 0;
            LoadLevel();
            ObjectPool.Init(10);
        }

        private void Start()
        {
            GameState = GameState.Loading;
        }

        //Public Methods
        public void ProgressLevel()
        {
            m_CurrentLevelIndex++;
            CurrentLevel.TileParent.SetParent(TileContainer.transform, true);
            LoadLevel();
        }

        public void RestartLevel()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            if (CurrentLevel)
            {
                Destroy(CurrentLevel.gameObject);
            }

            var levelIndex = m_CurrentLevelIndex % Levels.Count;
            var level = Levels[levelIndex];

            CurrentLevel = m_LevelFactory.Create(level);
        }
    }
}
