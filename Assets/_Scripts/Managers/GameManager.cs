using Assets._Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private int m_CurrentLevelIndex;

        private static GameManager m_Instance = null;
        public static GameManager Instance => m_Instance;

        [NonSerialized]
        public Level CurrentLevel;

        public TileContainer TileContainer;

        public Action<GameState> GameStateChanged;

        public GameplaySettings GameplaySettings;

        public List<Level> Levels;

        public TileSplitObjectPool ObjectPool;

        public CharacterController Character;

        public AudioController Audio;

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
            m_Instance = this;

            m_CurrentLevelIndex = 0;
            LoadLevel();
            ObjectPool.Init(10);
        }

        private void Start()
        {
            GameState = GameState.Loading;
        }

        private void Update()
        {
            if (GameState == GameState.Playing)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameState = GameState.GameOver;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    GameState = GameState.Success;
                }
            }
        }

        private void OnDestroy()
        {
            m_Instance = null;
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

            //TODO: make shuffle
            var levelIndex = m_CurrentLevelIndex % Levels.Count;
            var level = Levels[levelIndex];

            CurrentLevel = Instantiate(level);
        }
    }
}
