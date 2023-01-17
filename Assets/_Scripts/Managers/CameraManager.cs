using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager m_Instance = null;
        public static CameraManager Instance => m_Instance;

        public CinemachineVirtualCamera VirtualCam;

        private void Awake()
        {
            m_Instance = this;
        }

        private void OnDestroy()
        {
            m_Instance = null;
        }
    }
}
