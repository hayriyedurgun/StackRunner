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
        private int m_Priority;
        [SerializeField]
        private CinemachineBrain m_CinemachineBrain;

        private static CameraManager m_Instance = null;
        public static CameraManager Instance => m_Instance;

        public CinemachineVirtualCamera VirtualCam;
        public CinemachineVirtualCamera WinCam;

        private void Awake()
        {
            m_Instance = this;
        }

        private void OnDestroy()
        {
            m_Instance = null;
        }

        public void ChangeCam(CinemachineVirtualCamera cam, float time)
        {
            if (time != 0f)
            {
                m_CinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, time);
            }
            else
            {
                m_CinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
            }

            cam.Priority = ++m_Priority;
        }
    }
}
