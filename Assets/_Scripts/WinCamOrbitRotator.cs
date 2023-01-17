using Assets._Scripts.Managers;
using Assets._Scripts.ScriptableObjects;
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
        public GameplaySettings Settings => GameManager.Instance.GameplaySettings;

        private void Update()
        {
            transform.RotateAround(GameManager.Instance.Character.transform.position, Vector3.up, Settings.CamRotationSpeed
             * Time.deltaTime);
        }
    }
}
