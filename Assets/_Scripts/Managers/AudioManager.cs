using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        private int m_Counter = 0;

        [SerializeField]
        private AudioSource m_Audio;

        private static AudioManager m_Instance = null;
        public static AudioManager Instance => m_Instance;

        private void Awake()
        {
            m_Instance = this;
        }

        private void OnDestroy()
        {
            m_Instance = null;
        }

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
