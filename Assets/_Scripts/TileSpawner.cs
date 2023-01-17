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
        private float m_TileLenght = 4;
        private float m_FinishTileLen = 1.95f;

        public TileController TilePrefab;
        public TileController FinishTilePrefab;

        private void Start()
        {
            m_CurrentDistance += m_TileLenght;
        }

        public TileController Spawn(TileController previousTile)
        {
            var tile = Instantiate(TilePrefab, GameManager.Instance.CurrentLevel.transform);
            tile.transform.localScale = previousTile.transform.localScale;
            var direction = m_CurrentDistance % (m_TileLenght * 2) == 0 ? 1 : -1;

            var pos = tile.transform.position;
            pos.x = GameManager.Instance.GameplaySettings.SpawnX * direction;
            pos.z = m_CurrentDistance;
            tile.transform.position = pos;

            tile.Init(tile);

            m_CurrentDistance += m_TileLenght;

            return tile;
        }

        public TileController CreateFinish(TileController previousTile)
        {
            var tile = Instantiate(FinishTilePrefab, GameManager.Instance.CurrentLevel.transform);

            var pos = tile.transform.position;
            pos.x = previousTile.transform.position.x;
            pos.z = m_CurrentDistance - m_TileLenght + (m_FinishTileLen * 1.5f);
            tile.transform.position = pos;

            return tile;
        }
    }
}
