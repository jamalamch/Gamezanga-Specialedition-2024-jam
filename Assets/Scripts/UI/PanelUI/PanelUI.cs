using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace UIParty
{
    public class PanelUI : MonoBehaviour
    {
        [FoldoutGroup("Transition"), SerializeField] Image _bagroundImage;
        [FoldoutGroup("Transition"), SerializeField] private RectTransform _rect;

        [FoldoutGroup("Transition"), SerializeField] protected float _duration;
        [FoldoutGroup("Transition"), SerializeField] Vector2 _closePanel = new Vector2(0,-400);
        [FoldoutGroup("Transition"), SerializeField] Vector2 _openPanel;
        [FoldoutGroup("Transition"), SerializeField] bool _scale = false;
        [FoldoutGroup("Transition"), SerializeField] bool _move = true;
        [FoldoutGroup("Transition"), SerializeField] float _closeScale = 0;
        [FoldoutGroup("Transition"), SerializeField] Ease _scaleOpenEaseType = Ease.Linear;
        [FoldoutGroup("Transition"), SerializeField] Ease _scaleCloseEaseType = Ease.Linear;

        [SerializeField] protected ButtonUI _closeButtonUI;

        protected bool _open;
        protected bool _onUpdate;

        public System.Action actionOnOpend;
        public System.Action actionOnClosed;

        public RectTransform Rect => _rect;
        public bool isOpen => _open;

        protected virtual float _BagroundImageFade => 0.9f;

        public virtual void Init()
        {
            gameObject.SetActive(true);

            if (_move)
                _rect.anchoredPosition = _closePanel;


            if (_scale)
                _rect.localScale = Vector3.one * _closeScale;

            if (_bagroundImage)
            {
                _bagroundImage.DOFade(0, 0);
                _bagroundImage.raycastTarget = false;
                _bagroundImage.gameObject.SetActive(false);
            }
            _rect.gameObject.SetActive(false);

            if (_closeButtonUI)
                _closeButtonUI.Init(Close);
        }

        public void Open(Vector2 AtPosition)
        {
            _openPanel = AtPosition;
            Open();
        }

        public virtual void Open()
        {
            print("Open Panel " + name);
            if (!_open)
            {


                if (_bagroundImage)
                {
                    _bagroundImage.DOFade(_BagroundImageFade, _duration * 0.5f);
                    _bagroundImage.gameObject.SetActive(true);
                    _bagroundImage.raycastTarget = true;
                }

                _rect.gameObject.SetActive(true);

                if (_move)
                    _rect.DOAnchorPos(_openPanel, _duration).SetId(this).OnComplete(Opend);
                else
                {
                    DOVirtual.DelayedCall(_duration, Opend).SetId(this);
                }

                if (_scale)
                    _rect.DOScale(1, _duration).SetEase(_scaleOpenEaseType).SetId(this);
                _open = true;
            }
        }

        public virtual void Close()
        {
            if (_onUpdate)
                return;
            if (_open)
            {
                DOTween.Kill(this, true);

                if (_bagroundImage)
                {
                    _bagroundImage.DOFade(0, _duration * 0.5f);
                    _bagroundImage.raycastTarget = false;
                }

                if (_move)
                    _rect.DOAnchorPos(_closePanel, _duration).SetId(this).OnComplete(Closed);
                else
                {
                    DOVirtual.DelayedCall(_duration, Closed).SetId(this);
                }

                if (_scale)
                    _rect.DOScale(_closeScale, _duration).SetEase(_scaleCloseEaseType).SetId(this);
                _open = false;

            }
        }

        void Closed()
        {
            if(_bagroundImage)
                _bagroundImage.gameObject.SetActive(false);
            _rect.gameObject.SetActive(false);

            actionOnClosed?.Invoke();
        }

        void Opend()
        {
            actionOnOpend?.Invoke();
        }
    }
}
