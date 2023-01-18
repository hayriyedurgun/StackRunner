﻿using Assets._Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        private TileController m_CurrentTile;
        public TileController CurrentTile
        {
            get => m_CurrentTile;
            private set
            {
                m_CurrentTile = value;
                m_TileCounter++;
            }
        }

        private void Start()
        {
            GameManager.Instance.Character.OnLevelStarted();
            CameraManager.Instance.ChangeCam(CameraManager.Instance.VirtualCam, 0f);
            CurrentTile = m_InitialTile;

            if (GameManager.Instance.GameState == GameState.Loading)
            {
                GameManager.Instance.GameStateChanged += OnStateChanged;
            }
            else
            {
                var prev = CurrentTile;

                CurrentTile = m_Spawner.Spawn(prev);
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

                CurrentTile = m_Spawner.Spawn(prev);
                CurrentTile.TilePlaced += OnTilePlaced;
            }
        }

        private void OnTilePlaced()
        {
            CurrentTile.TilePlaced -= OnTilePlaced;

            var prev = m_CurrentTile;
            if (m_TileCounter == TileCount)
            {
                CurrentTile = m_Spawner.CreateFinish(prev);
            }
            else if (GameManager.Instance.GameState == GameState.Playing)
            {

                CurrentTile = m_Spawner.Spawn(prev);
                CurrentTile.TilePlaced += OnTilePlaced;
            }
        }
    }
}
