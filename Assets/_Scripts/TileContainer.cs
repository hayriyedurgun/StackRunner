using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class TileContainer : MonoBehaviour
    {
        private float m_LastDistance = 4;
        private Vector3 m_CheckpointPos;

        public float CurrentDistance { get; set; }

        public void SetCheckPoint(Vector3 position)
        {
            m_LastDistance = CurrentDistance;
            m_CheckpointPos = position;
        }

        public Vector3 RecoverPosition()
        {
            CurrentDistance = m_LastDistance;
            return m_CheckpointPos;
        }
    }
}
