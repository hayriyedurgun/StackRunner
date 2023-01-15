using Assets._Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.UI
{
    public class SuccessPanel : BasePanel
    {
        public override void OnClick()
        {
            //TODO!!!
            GameManager.Instance.GameState = GameState.Playing;
            GameManager.Instance.ProgressLevel();
        }
    }
}
