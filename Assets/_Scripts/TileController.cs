using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Scripts
{
    public class TileController : BaseTileController
    {
        private Tween m_Tween;
        private Material m_CurrentMaterial;
        private BaseTileController m_PreviousTile;
        [SerializeField]
        private Renderer m_Renderer;
        [SerializeField]
        private MaterialLibrary m_MaterialLibrary;
        [SerializeField]
        private CollectibleLibrary m_CollectibleLibrary;

        public override float TileSize { get; protected set; } = 4f;

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

        private void OnDestroy()
        {
            m_Tween?.Kill();
        }

        public void Init(BaseTileController previousTile)
        {
            m_PreviousTile = previousTile;

            m_Tween = transform.DOMoveX(transform.position.x * -1, Settings.YoyoDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Settings.Ease);

            m_CurrentMaterial = m_MaterialLibrary.GetMaterial();
            m_Renderer.sharedMaterial = m_CurrentMaterial;

            var collectible = m_CollectibleLibrary.GetRandomCollectible();
            if (collectible != null)
            {
                collectible = Instantiate(collectible);
                collectible.Initialize(transform);
            }
        }

        private void Place()
        {
            m_Tween?.Kill();
            m_Tween = null;

            var splitDiff = transform.position.x - m_PreviousTile.transform.position.x;

            if (Math.Abs(splitDiff) >= m_PreviousTile.transform.localScale.x)
            {
                Destroy(gameObject);
                GameManager.Instance.GameState = GameState.PostGameOver;
            }
            else
            {
                Split(splitDiff);
                OnPlaced();
            }
        }

        private void Split(float diff)
        {
            var newSize = m_PreviousTile.transform.localScale.x - Mathf.Abs(diff);
            var fallingTileSize = transform.localScale.x - newSize;
            if (fallingTileSize > Settings.CutThreshold)
            {
                var direction = diff > 0 ? 1 : -1;

                var newPos = m_PreviousTile.transform.position.x + (diff / 2f);

                var scale = transform.localScale;
                scale.x = newSize;
                transform.localScale = scale;

                var pos = transform.position;
                pos.x = newPos;
                transform.position = pos;

                var edge = transform.position.x + (newSize / 2f * direction);
                var fallingPos = edge + (fallingTileSize / 2f * direction);

                var splitTile = GameManager.Instance.ObjectPool.GetObject();
                splitTile.gameObject.SetActive(true);
                splitTile.SetMaterial(m_CurrentMaterial);

                scale = transform.localScale;
                scale.x = fallingTileSize;
                splitTile.transform.localScale = scale;

                pos = transform.position;
                pos.x = fallingPos;
                splitTile.transform.position = pos;

                StartCoroutine(Release(splitTile));
                GameManager.Instance.Audio.Reset();
            }
            else
            {
                GameManager.Instance.Audio.Play();
            }
        }

        private IEnumerator Release(TileSplitController tile)
        {
            yield return new WaitForSeconds(2f);
            GameManager.Instance.ObjectPool.ReleaseObject(tile);
        }
    }
}
