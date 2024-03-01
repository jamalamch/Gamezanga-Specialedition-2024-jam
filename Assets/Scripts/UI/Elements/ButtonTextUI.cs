using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace UIParty
{
    public class ButtonTextUI : ButtonUI
    {
        [SerializeField] private TextMeshProUGUI _displayText;
        public void SetDisplay(string display)
        {
            Precondition.CheckNotNull(display);
            _displayText.text = display;
        }
    }
}
