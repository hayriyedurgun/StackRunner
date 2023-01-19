using Assets._Scripts.Factories;
using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using static Assets._Scripts.TileController;

namespace Assets._Scripts
{
    public class TileSpawner : MonoBehaviour
    {
        [Inject]
        private TileFactory m_TileFactory;
        [Inject]
        private FinishTileFactory m_FinishTileFactory;

        public TileController Spawn(BaseTileController previousTile, Transform parent)
        {
            var container = GameManager.Instance.TileContainer;
            //var tile = Instantiate(TilePrefab, GameManager.Instance.CurrentLevel.TileParent);
            var tile = m_TileFactory.Create();
            tile.transform.SetParent(parent);

            tile.transform.localScale = previousTile.transform.localScale;
            var direction = container.CurrentDistance % (tile.TileSize * 2) == 0 ? 1 : -1;

            var pos = tile.transform.position;
            pos.x = GameManager.Instance.GameplaySettings.SpawnX * direction;
            pos.z = container.CurrentDistance;
            tile.transform.position = pos;

            tile.Init(previousTile);

            container.CurrentDistance += tile.TileSize;

            return tile;
        }

        public FinishTileController CreateFinish(BaseTileController previousTile, Transform parent)
        {
            var container = GameManager.Instance.TileContainer;

            var tile = m_FinishTileFactory.Create();
            tile.transform.SetParent(parent);

            var pos = tile.transform.position;
            pos.x = previousTile.transform.position.x;
            pos.z = container.CurrentDistance - (previousTile.TileSize / 2) + (tile.TileSize / 2);
            tile.transform.position = pos;

            container.CurrentDistance = pos.z + (previousTile.TileSize / 2) + (tile.TileSize / 2);
            return tile;
        }
    }
}
