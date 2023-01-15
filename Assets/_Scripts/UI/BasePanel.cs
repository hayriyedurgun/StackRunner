using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.UI
{
    public abstract class BasePanel : MonoBehaviour
    {
        public virtual void ChangeVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        public virtual void OnClick()
        {
        }
    }
}
