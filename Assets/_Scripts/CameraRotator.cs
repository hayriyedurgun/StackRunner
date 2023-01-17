using Assets._Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class CameraRotator : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * GameManager.Instance.GameplaySettings.CamRotationSpeed);
        }
    }
}
