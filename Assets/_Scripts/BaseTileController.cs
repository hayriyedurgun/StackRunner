using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public abstract class BaseTileController : MonoBehaviour
    {
        public event Action TilePlaced;

        public abstract float TileSize { get; protected set; }

        public virtual Vector3 GetEdge()
        {
           return transform.position - transform.forward * (TileSize / 2f);
        }

        protected void OnPlaced()
        {
            TilePlaced?.Invoke();
        }
    }
}
