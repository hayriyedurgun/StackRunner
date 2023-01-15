using Assets._Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.UI
{
    public class GameOverPanel : BasePanel
    {
        public override void OnClick()
        {
            GameManager.Instance.GameState = GameState.Playing;
            GameManager.Instance.RestartLevel();
        }
    }
}
