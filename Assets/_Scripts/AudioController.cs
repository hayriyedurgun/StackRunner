using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class AudioController : MonoBehaviour
    {
        private int m_Counter = 0;

        [SerializeField]
        private AudioSource m_Audio;

        public void Play()
        {
            m_Audio.pitch = 0.5f + m_Counter * 0.2f;
            m_Audio.Play();
            m_Counter++;
        }

        public void Reset()
        {
            m_Counter = 0;

            m_Audio.pitch = 0.5f + m_Counter * 0.2f;
            m_Audio.Play();
            m_Counter++;
        }
    }
}
