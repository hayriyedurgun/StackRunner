using Assets._Scripts.Managers;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets._Scripts.UI
{
    public class SuccessPanel : BasePanel
    {
        public TextMeshProUGUI Text;

        private void Start()
        {
            Text.transform.DOScale(1.25f, .5f).SetLoops(-1, LoopType.Yoyo);
        }

        public override void OnClick()
        {
            GameManager.Instance.GameState = GameState.Playing;
            GameManager.Instance.ProgressLevel();
        }
    }
}
