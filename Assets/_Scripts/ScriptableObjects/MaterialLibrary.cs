using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MaterialLibrary", menuName = "ScriptableObjects/MaterialLibrary", order = 1)]
    public class MaterialLibrary : ScriptableObject
    {
        [NonSerialized]
        private int m_Counter = 0;

        public List<Material> Materials;

        public Material GetMaterial()
        {
            var mat = Materials[m_Counter % Materials.Count];
            m_Counter++;

            return mat;
        }

    }
}
