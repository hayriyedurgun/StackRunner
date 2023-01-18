using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class CollectibleBehaviour : MonoBehaviour
    {
        private Transform m_FollowTransform;
        private bool m_Initialized;
        protected Tween m_ScaleTween;
        protected Tween m_RotateTween;

        public ParticleSystem Particle;
        public bool CanScale;
        public bool CanRotate;

        private void Start()
        {
            if (CanScale)
            {
                m_ScaleTween = transform.DOScale(Vector3.one * .25f, .5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
            }
            if (CanScale)
            {
                m_RotateTween = transform.DORotate(Vector3.up * 25, .1f).SetLoops(-1, LoopType.Incremental);
            }
        }

        private void Update()
        {
            if (m_Initialized)
            {
                if (m_FollowTransform)
                {
                    transform.position = m_FollowTransform.position + Vector3.up;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            m_RotateTween?.Kill();
            m_ScaleTween?.Kill();
        }

        public void Initialize(Transform followTransform)
        {
            m_Initialized = true;
            m_FollowTransform = followTransform;
        }

        public void Collect()
        {
            m_ScaleTween?.Kill();
            m_RotateTween?.Kill();

            Particle.Play();
            transform.DOJump(Vector3.up * 3, 20, 1, .25f)
                .SetEase(Ease.OutExpo)
                .SetRelative(true)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}
