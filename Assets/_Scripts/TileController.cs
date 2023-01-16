using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class TileController : MonoBehaviour
    {
        private Tween m_Tween;
        private TileController m_PreviousTile;

        public Action TilePlaced;


        public GameplaySettings Settings => GameManager.Instance.GameplaySettings;

        private void Update()
        {
            if (GameManager.Instance.GameState != GameState.Playing ||
                m_Tween == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                Place();
                return;
            }
        }

        public void Init(TileController previousTile)
        {
            m_PreviousTile = previousTile;

            var x = Settings.SpawnX * -1;
            m_Tween = transform.DOMoveX(x, Settings.YoyoDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Settings.Ease);
        }

        private void Place()
        {
            var splitDiff = transform.position.x - m_PreviousTile.transform.position.x;
            if (Math.Abs(splitDiff) >= m_PreviousTile.transform.localScale.x)
            {
                Destroy(gameObject);
                GameManager.Instance.GameState = GameState.GameOver;
                //TODO: gameover
            }
            else
            {
                m_Tween?.Kill();
                m_Tween = null;

                Split(splitDiff);

                TilePlaced?.Invoke();
            }
        }

        private void Split(float diff)
        {
            var newSize = m_PreviousTile.transform.localScale.x - Mathf.Abs(diff);
            var direction = diff > 0 ? 1 : -1;
            var fallingTileSize = transform.localScale.x - newSize;

            var newPos = m_PreviousTile.transform.position.x + (diff / 2f);

            var scale = transform.localScale;
            scale.x = newSize;
            transform.localScale = scale;

            var pos = transform.position;
            pos.x = newPos;
            transform.position = pos;

            var edge = transform.position.x + (newSize / 2f * direction);
            var fallingPos = edge + (fallingTileSize / 2f * direction);
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(GameManager.Instance.CurrentLevel.transform);

            scale = transform.localScale;
            scale.x = fallingTileSize;
            cube.transform.localScale = scale;

            pos = transform.position;
            pos.x = fallingPos;
            cube.transform.position = pos;

            cube.AddComponent<Rigidbody>();
            Destroy(cube.gameObject, 2f);
        }
    }
}
