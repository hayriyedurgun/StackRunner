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
    public class TileSpawner : MonoBehaviour
    {
        private float m_CurrentDistance = 0f;
        private TileController m_CurrentTile;

        public float TileLenght = 2;
        public TileController TilePrefab;
        public TileController InitialTile;

        private void Start()
        {
            m_CurrentDistance += TileLenght;
            GameManager.Instance.GameStateChanged += OnStateChanged;
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

        public void Spawn()
        {
            if (m_CurrentTile == null)
            {
                m_CurrentTile = InitialTile;
            }

            var tile = Instantiate(TilePrefab, GameManager.Instance.CurrentLevel.transform);
            tile.transform.localScale = m_CurrentTile.transform.localScale;
            var direction = m_CurrentDistance % (TileLenght * 2) == 0 ? 1 : -1;

            var pos = tile.transform.position;
            //pos.x = tile.transform.localScale.x * direction;
            pos.x = GameManager.Instance.GameplaySettings.SpawnX * direction;
            pos.z = m_CurrentDistance;
            tile.transform.position = pos;

            tile.Init(m_CurrentTile);

            m_CurrentTile = tile;
            m_CurrentTile.TilePlaced += OnTilePlaced;

            m_CurrentDistance += TileLenght;
        }

        private void OnStateChanged(GameState state)
        {
            if (state == GameState.Playing)
            {
                GameManager.Instance.GameStateChanged -= OnStateChanged;
                Spawn();
            }
        }

        private void OnTilePlaced()
        {
            m_CurrentTile.TilePlaced -= OnTilePlaced;
            Spawn();
        }
    }
}
