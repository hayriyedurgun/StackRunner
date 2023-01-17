using Assets._Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class Level : MonoBehaviour
    {
        private int m_TileCounter;
        private TileController m_FinishTile;

        [SerializeField]
        private TileSpawner m_Spawner;
        [SerializeField]
        private TileController m_InitialTile;

        public int TileCount = 5;

        private TileController m_CurrentTile;
        public TileController CurrentTile
        {
            get => m_CurrentTile;
            private set
            {
                m_CurrentTile = value;
                m_TileCounter++;
            }
        }

        private void Start()
        {
            CameraManager.Instance.ChangeCam(CameraManager.Instance.VirtualCam, 0f);
            GameManager.Instance.GameStateChanged += OnStateChanged;

            m_FinishTile = m_Spawner.CreateFinish(TileCount);
            CurrentTile = m_InitialTile;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.GameStateChanged -= OnStateChanged;
            }

            if (m_CurrentTile)
            {
                m_CurrentTile.TilePlaced -= OnTilePlaced;
            }
        }

        private void OnStateChanged(GameState state)
        {
            if (state == GameState.Playing)
            {
                GameManager.Instance.GameStateChanged -= OnStateChanged;

                var prev = m_CurrentTile;

                CurrentTile = m_Spawner.Spawn(prev);
                CurrentTile.TilePlaced += OnTilePlaced;
            }
        }

        private void OnTilePlaced()
        {
            CurrentTile.TilePlaced -= OnTilePlaced;

            if (m_TileCounter == TileCount)
            {
                CurrentTile = m_FinishTile;
            }
            else if (GameManager.Instance.GameState == GameState.Playing)
            {
                var prev = m_CurrentTile;

                CurrentTile = m_Spawner.Spawn(prev);
                CurrentTile.TilePlaced += OnTilePlaced;
            }
        }
    }
}
