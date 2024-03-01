using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


namespace UIParty
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] protected RectTransform _rect;
        [SerializeField] protected Image _transparentImageFill;
        
        public RectTransform Rect => _rect;

        public void SetProgressionRaw (float progression)
        {
            Precondition.CheckNotNull(progression);
            
            _transparentImageFill.DOKill();
            _transparentImageFill.fillAmount = progression;
        }

        public void SetProgression (float progression)
        {
            Precondition.CheckNotNull(progression);

            if (progression == 0)
            {
                SetProgressionRaw(progression);
                return;
            }

            _transparentImageFill.DOComplete();
            _transparentImageFill.DOFillAmount(progression, .1f).SetEase(Ease.Linear);
        }

        protected virtual void OnValidate()
        {
            if(!_transparentImageFill)
            {
                var images = GetComponentsInChildren<Image>();
                foreach (var imag in images)
                {
                    if(imag.type == Image.Type.Filled)
                    {
                        _transparentImageFill = imag;
                        break;
                    }
                }
                _rect = (RectTransform)transform;
            }
        }
    }
}