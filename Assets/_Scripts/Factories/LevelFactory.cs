using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets._Scripts.Factories
{
    public class LevelFactory : PrefabFactory<Level>
    {
        //private int m_Index;
        //public List<Level> LevelPrefabs;

        //public override Level Create(UnityEngine.Object prefab)
        //{
        //    var temp = m_Index % LevelPrefabs.Count;
        //    m_Index++;
        //    return base.Create(LevelPrefabs[temp]);
        //}

    }
}
