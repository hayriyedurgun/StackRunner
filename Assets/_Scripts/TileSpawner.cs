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
        private float m_TileLenght = 4;
        private float m_FinishTileLen = 1.8f;

        public TileController TilePrefab;
        public TileController FinishTilePrefab;

        private void Start()
        {
        }

        public TileController Spawn(TileController previousTile)
        {
            var container = GameManager.Instance.TileContainer;
            var tile = Instantiate(TilePrefab, GameManager.Instance.CurrentLevel.TileParent);
            tile.transform.localScale = previousTile.transform.localScale;
            var direction = container.CurrentDistance % (m_TileLenght * 2) == 0 ? 1 : -1;

            var pos = tile.transform.position;
            pos.x = GameManager.Instance.GameplaySettings.SpawnX * direction;
            pos.z = container.CurrentDistance;
            tile.transform.position = pos;

            tile.Init(previousTile);

            container.CurrentDistance += m_TileLenght;

            return tile;
        }

        public TileController CreateFinish(TileController previousTile)
        {
            var container = GameManager.Instance.TileContainer;

            var tile = Instantiate(FinishTilePrefab, GameManager.Instance.CurrentLevel.TileParent);

            var pos = tile.transform.position;
            pos.x = previousTile.transform.position.x;
            pos.z = container.CurrentDistance - (m_TileLenght / 2) + (m_FinishTileLen / 2);
            tile.transform.position = pos;

            container.CurrentDistance = pos.z + (m_TileLenght / 2) + (m_FinishTileLen / 2);
            return tile;
        }
    }
}
