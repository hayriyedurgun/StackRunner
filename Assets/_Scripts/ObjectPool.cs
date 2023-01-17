using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        private List<T> m_PooledObjects;

        private static ObjectPool<T> m_Instance = null;
        public static ObjectPool<T> Instance => m_Instance;

        public T ObjectToPool;

        public Transform ItemsParent;

        private void Awake()
        {
            m_Instance = this;
        }

        private void OnDestroy()
        {
            m_Instance = null;
        }

        public T GetObject()
        {
            foreach (var obj in m_PooledObjects)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    return obj;
                }
            }

            //for dynamic pooling
            var temp = Instantiate(ObjectToPool);
            temp.gameObject.SetActive(false);
            m_PooledObjects.Add(temp);

            return temp;
        }

        public virtual void ReleaseObject(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(null);
            obj.transform.position = Vector3.zero;
        }

        public void Init(int count)
        {
            m_PooledObjects = new List<T>(count);
            T temp;

            for (int i = 0; i < count; i++)
            {
                temp = Instantiate(ObjectToPool);
                temp.gameObject.SetActive(false);
                temp.transform.SetParent(ItemsParent);

                m_PooledObjects.Add(temp);
            }
        }
    }
}
