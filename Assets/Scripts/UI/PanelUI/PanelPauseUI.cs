using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIParty
{
    public class PanelPauseUI : PanelUI
    {
        [SerializeField] ButtonUI _homeButton;

        public override void Init()
        {
            base.Init();
            _homeButton.Init(GoHome);
        }

        void GoHome()
        {
            Close();
            //Root.GameManager.ResetGame();
        }
    }
}