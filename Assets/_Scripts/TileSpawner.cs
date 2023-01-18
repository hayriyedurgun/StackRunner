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
        public TileController TilePrefab;
        public FinishTileController FinishTilePrefab;

        public TileController Spawn(BaseTileController previousTile)
        {
            var container = GameManager.Instance.TileContainer;
            var tile = Instantiate(TilePrefab, GameManager.Instance.CurrentLevel.TileParent);
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

        public FinishTileController CreateFinish(BaseTileController previousTile)
        {
            var container = GameManager.Instance.TileContainer;

            var tile = Instantiate(FinishTilePrefab, GameManager.Instance.CurrentLevel.TileParent);

            var pos = tile.transform.position;
            pos.x = previousTile.transform.position.x;
            pos.z = container.CurrentDistance - (TilePrefab.TileSize / 2) + (tile.TileSize / 2);
            tile.transform.position = pos;

            container.CurrentDistance = pos.z + (TilePrefab.TileSize / 2) + (tile.TileSize / 2);
            return tile;
        }
    }
}
