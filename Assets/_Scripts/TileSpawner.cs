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

        public float TileLenght = 2;
        public TileController TilePrefab;
        public TileController FinishTilePrefab;

        private void Start()
        {
            m_CurrentDistance += TileLenght;
        }

        public TileController Spawn(TileController previousTile)
        {
            var tile = Instantiate(TilePrefab, GameManager.Instance.CurrentLevel.transform);
            tile.transform.localScale = previousTile.transform.localScale;
            var direction = m_CurrentDistance % (TileLenght * 2) == 0 ? 1 : -1;

            var pos = tile.transform.position;
            pos.x = GameManager.Instance.GameplaySettings.SpawnX * direction;
            pos.z = m_CurrentDistance;
            tile.transform.position = pos;

            tile.Init(tile);

            m_CurrentDistance += TileLenght;

            return tile;
        }

        public TileController CreateFinish(int totalCount)
        {
            var tile = Instantiate(FinishTilePrefab, GameManager.Instance.CurrentLevel.transform);
            
            var pos = tile.transform.position;
            pos.z = TileLenght * totalCount - TileLenght / 4;
            tile.transform.position = pos;

            return tile;
        }
    }
}
