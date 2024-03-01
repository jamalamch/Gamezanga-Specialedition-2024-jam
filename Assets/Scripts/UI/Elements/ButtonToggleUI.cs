using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIParty
{
    public class ButtonToggleUI : MonoBehaviour
    {

        [SerializeField] private RectTransform _rect;
        [SerializeField] private Button _button;

        [SerializeField] GameObject onObj, ofObj;

        public bool On { get; private set; }
        public RectTransform Rect => _rect;


        public void Init(Action<bool> callback,bool defauValue)
        {
            SetOn(defauValue);
            _button.onClick.AddListener(() => 
            {
                SetOn(!On);
                AudioManager.Play("Click");
                callback(On);
            });
        }

        public void SetOn(bool isOn)
        {
            On = isOn;
            onObj.SetActive(On);
            if(ofObj)
                ofObj.SetActive(!On);
        }

        private void OnValidate()
        {
            if (!_rect) _rect = (RectTransform)transform;
            if (!_button) _button = GetComponentInChildren<Button>();
        }
    }
}
