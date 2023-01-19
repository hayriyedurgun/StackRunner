using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class CameraController : MonoBehaviour
    {
        private int m_Priority;

        [SerializeField]
        private CinemachineBrain m_CinemachineBrain;
        public CinemachineVirtualCamera VirtualCam;
        public CinemachineVirtualCamera WinCam;

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
