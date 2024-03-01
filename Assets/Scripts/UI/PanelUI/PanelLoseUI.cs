using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIParty
{
    public class PanelLoseUI : PanelUI
    {
        [SerializeField] ButtonUI _restartButton;

        public override void Init()
        {
            base.Init();
            _closeButtonUI.Init(Lose);
            _restartButton.Init(Restart);
        }

        void Restart()
        {
            Close();
            //Root.GameManager.ResetGame();
          //  AdsController.Instance.ShowAdsIdIntertial();
        }

        void Lose()
        {
            Close();
         //   AdsController.Instance.ShowAdsIdIntertial();
        }
    }
}