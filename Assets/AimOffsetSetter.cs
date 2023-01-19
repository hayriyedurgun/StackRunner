using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimOffsetSetter : MonoBehaviour
{
    private CinemachineVirtualCamera m_Cam;
    private CinemachineComposer m_Composer;

    private void Awake()
    {
        m_Cam = GetComponent<CinemachineVirtualCamera>();
        m_Composer = m_Cam.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        if (m_Cam.LookAt)
        {
            var targetPos = m_Cam.LookAt.transform.position;
            var pos = new Vector3(.75f, 2.5f, 1.21f);

            targetPos += pos;
            m_Composer.m_TrackedObjectOffset = m_Cam.LookAt.InverseTransformPoint(targetPos);
        }
    }
}
