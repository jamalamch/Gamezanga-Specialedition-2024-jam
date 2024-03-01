using UnityEngine;
using TMPro;

namespace UIParty
{
    public class DisplayUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TextMeshProUGUI _displayText;

        public RectTransform Rect => _rect;

        public void SetDisplay(string display)
        {
            _displayText.text = display;
        }

        public void SetColor(Color color)
        {
            _displayText.color = color;
        }

        private void OnValidate()
        {
            if (!_displayText)
            {
                _displayText = GetComponentInChildren<TextMeshProUGUI>();
                if (_displayText)
                    _rect = (RectTransform)_displayText.transform;
            }
        }
    }
}