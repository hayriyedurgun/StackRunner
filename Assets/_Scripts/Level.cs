using Assets._Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets._Scripts
{
    public class Level : MonoBehaviour
    {
        private int m_TileCounter;

        [SerializeField]
        private TileSpawner m_Spawner;
        [SerializeField]
        private TileController m_InitialTile;

        public int TileCount = 5;
        public Transform TileParent;

        private BaseTileController m_CurrentTile;
        public BaseTileController CurrentTile
        {
            get => m_CurrentTile;
            private set
            {
                m_CurrentTile = value;
                m_TileCounter++;
            }
        }

        public Queue<Vector3> TileEdges = new Queue<Vector3>();

        private void Start()
        {
            GameManager.Instance.Character.OnLevelStarted();
            CurrentTile = m_InitialTile;

            if (GameManager.Instance.GameState == GameState.Loading)
            {
                GameManager.Instance.GameStateChanged += OnStateChanged;
            }
            else
            {
                var prev = CurrentTile;
                CurrentTile = m_Spawner.Spawn(prev, TileParent);
                CurrentTile.TilePlaced += OnTilePlaced;
            }
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

                CurrentTile = m_Spawner.Spawn(prev, TileParent);
                CurrentTile.TilePlaced += OnTilePlaced;
            }
        }

        private void OnTilePlaced()
        {
            CurrentTile.TilePlaced -= OnTilePlaced;

            var prev = m_CurrentTile;
            if (m_TileCounter == TileCount)
            {
                CurrentTile = m_Spawner.CreateFinish(prev, TileParent);
            }
            else if (GameManager.Instance.GameState == GameState.Playing)
            {
                CurrentTile = m_Spawner.Spawn(prev, TileParent);
                CurrentTile.TilePlaced += OnTilePlaced;
            }

            var edge = prev.GetEdge();
            if (edge.z > 0)
            {
                TileEdges.Enqueue(edge);
            }

        }
    }
}
