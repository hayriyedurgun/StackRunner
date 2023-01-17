using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class TileSplitController : MonoBehaviour
    {
        [SerializeField]
        private Renderer m_Renderer;

        public void SetMaterial(Material mat)
        {
            m_Renderer.material = mat;
        }
    }
}
