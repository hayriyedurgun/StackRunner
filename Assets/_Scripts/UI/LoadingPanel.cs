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
    public class LoadingPanel : BasePanel
    {
        public TextMeshProUGUI TapToPlay;

        private void Start()
        {
            TapToPlay.transform.DOScale(1.25f, .5f).SetLoops(-1, LoopType.Yoyo);
        }

        public override void OnClick()
        {
            //TODO: !!!
            GameManager.Instance.GameState = GameState.Playing;
        }
    }
}
