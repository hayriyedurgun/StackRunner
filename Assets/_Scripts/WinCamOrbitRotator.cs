using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class WinCamOrbitRotator : MonoBehaviour
    {
        private float m_Alpha;
        private CinemachineTransposer m_Transposer;

        public GameplaySettings Settings => GameManager.Instance.GameplaySettings;

        private void Start()
        {
            m_Transposer = CameraManager.Instance.WinCam.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void Update()
        {
            var z = Mathf.Sin(m_Alpha) * Settings.Radius;
            var x = Mathf.Cos(m_Alpha) * Settings.Radius;

            var offset = m_Transposer.m_FollowOffset;
            offset.x = x;
            offset.z = z;
            m_Transposer.m_FollowOffset = offset;

            m_Alpha += Settings.IncrementAlpha;
        }
    }
}
