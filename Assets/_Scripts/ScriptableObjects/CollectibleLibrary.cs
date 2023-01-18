using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CollectibleLibrary", menuName = "ScriptableObjects/CollectibleLibrary", order = 2)]
    public class CollectibleLibrary : ScriptableObject
    {
        public List<CollectibleBehaviour> Collectibles;

        public CollectibleBehaviour GetRandomCollectible()
        {
            var rand = UnityEngine.Random.Range(0, Collectibles.Count * 2);
            if (rand < Collectibles.Count)
            {
                return Collectibles[rand];
            }

            return null;
        }
    }
}
