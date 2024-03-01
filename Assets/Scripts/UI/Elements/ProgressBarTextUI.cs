using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UIParty
{
    public class ProgressBarTextUI : ProgressBarUI
    {
        [SerializeField] private TextMeshProUGUI _displayText;
        public void SetDisplay(string display)
        {
            Precondition.CheckNotNull(display);
            _displayText.text = display;
        }

        protected override void OnValidate()
        {
            if (!_displayText)
                _displayText = GetComponentInChildren<TextMeshProUGUI>();
            base.OnValidate();
        }
    }
}
