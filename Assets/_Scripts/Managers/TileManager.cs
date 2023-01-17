//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets._Scripts.Managers
//{
//    public class TileManager : MonoBehaviour
//    {
//        private static TileManager m_Instance = null;
//        public static TileManager Instance => m_Instance;

//        public TileSpawner Spawner;
//        public TileController InitialTile;
//        public TileController CurrentTile;

//        private void Awake()
//        {
//            m_Instance = this;
//        }

//        private void Start()
//        {
//            if (CurrentTile == null)
//            {
//                CurrentTile = InitialTile;
//            }

//            GameManager.Instance.GameStateChanged += OnStateChanged;
//        }

//        private void OnDestroy()
//        {
//            m_Instance = null;

//            if (GameManager.Instance)
//            {
//                GameManager.Instance.GameStateChanged -= OnStateChanged;
//            }
//        }

//        private void OnStateChanged(GameState state)
//        {
//            if (state == GameState.Playing)
//            {
//                GameManager.Instance.GameStateChanged -= OnStateChanged;
//                var prev = CurrentTile;
//                CurrentTile = Spawner.Spawn(prev);

//                CurrentTile.TilePlaced += OnTilePlaced;
//            }
//        }

//        private void OnTilePlaced()
//        {
//            CurrentTile.TilePlaced -= OnTilePlaced;

//            var prev = CurrentTile;
//            CurrentTile = Spawner.Spawn(prev);
//        }
//    }
//}
