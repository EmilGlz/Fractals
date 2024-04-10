using Assets.Scripts;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public abstract class PopupBase : IDisposable
    {
        private const string _defaultPrefabName = "Prefabs/PopupTemplate";
        protected RectTransform PopupRect;
        public GameObject Instance => PopupRect.gameObject;
        protected Vector2 AnimationStartPos;
        protected Vector2 AnimationEndPos;
        protected readonly FreeModifier CornerModifier;
        protected virtual float CloseDuration => 0.4f;
        protected virtual float OpenDuration => 0.4f;
        protected virtual Color? BackgroundColor => new Color(1, 1, 1, 0);
        protected PopupAlignment Alignment { get; private set; }
        protected VerticalLayoutGroup PopupVerticalLayoutGroup { get; }
        private Background _background;
        protected Func<PopupAlignment> _getAlignment;
        private readonly Action _onBack;
        private readonly Action _onClose;

        protected bool IsClosing { get; private set; }
        protected virtual RectOffset PopupPadding => Alignment switch
        {
            PopupAlignment.Bottom => new RectOffset(0, 0, 24, 24 + Device.BottomOffset),
            PopupAlignment.Top => new RectOffset(0, 0, 24 + Device.TopOffset, 24),
            _ => new RectOffset()
        };
        protected PopupBase(Func<PopupAlignment> getAlignment = null, Transform parent = null, Action onBack = null, Action onClose = null)
        {
            _background = new Background(Dispose);
            _getAlignment = getAlignment ?? (() => PopupAlignment.Bottom);
            _onBack = onBack;
            _onClose = onClose;
            PopupRect = ResourceHelper.LoadPrefab(_defaultPrefabName, parent != null ? parent : Main.Instance.PopupLayer).GetComponent<RectTransform>();
            PopupRect.name = GetType().Name;
            PopupRect.transform.SetAsLastSibling();
            CornerModifier = PopupRect.GetComponent<FreeModifier>();
            PopupVerticalLayoutGroup = PopupRect.gameObject.GetComponent<VerticalLayoutGroup>();
            Alignment = _getAlignment();
            PopupsManager.instance.DestroyLast();
            PopupsManager.instance.AddPopup(this);
            if(TouchRotation.Instance != null)
                TouchRotation.Instance.CanTouchRotate = false;
        }
        protected virtual void InitToolbar()
        {
            var topTb = Utils.FindGameObject("TopToolbar", PopupRect.gameObject).GetComponent<RectTransform>();
            topTb.SetHeight(36);
            var backButton = Utils.FindGameObject<Button>("LeftButton", PopupRect);
            var closeButton = Utils.FindGameObject<Button>("RightButton", PopupRect);
            backButton.gameObject.SetActive(_onBack != null);
            closeButton.gameObject.SetActive(_onClose != null);

            if (_onBack != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(() =>
                {
                    _onBack.Invoke();
                });
            }
            if (_onClose != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() =>
                {
                    _onClose.Invoke();
                });
            }

            if(_onClose == null && _onBack == null)
                topTb.SetHeight(0);
        }
        protected virtual void UpdateSizes()
        {
            if (PopupRect == null)
                return;

            CornerModifier.Radius = Alignment switch
            {
                PopupAlignment.Bottom => new Vector4(16, 16, 0, 0),
                PopupAlignment.Top => new Vector4(0, 0, 16, 16),
                PopupAlignment.Center => new Vector4(16, 16, 16, 16),
                _ => Vector4.zero
            };
            PopupVerticalLayoutGroup.padding = PopupPadding;
            switch (Alignment)
            {
                case PopupAlignment.Left:
                    PopupRect.anchorMin = new Vector2(0, 0);
                    PopupRect.anchorMax = new Vector2(0, 1);
                    PopupRect.pivot = new Vector2(0, 0);
                    break;
                case PopupAlignment.Right:
                    PopupRect.anchorMin = new Vector2(1, 0);
                    PopupRect.anchorMax = new Vector2(1, 1);
                    PopupRect.pivot = new Vector2(1, 0);
                    break;
                case PopupAlignment.Top:
                    PopupRect.anchorMin = new Vector2(0, 1);
                    PopupRect.anchorMax = new Vector2(1, 1);
                    PopupRect.pivot = new Vector2(0.5f, 1);
                    break;
                case PopupAlignment.Bottom:
                    PopupRect.anchorMin = new Vector2(0, 0);
                    PopupRect.anchorMax = new Vector2(1, 0);
                    PopupRect.pivot = new Vector2(0.5f, 0);
                    break;
                case PopupAlignment.Center:
                    PopupRect.anchorMin = new Vector2(0.5f, 0.5f);
                    PopupRect.anchorMax = new Vector2(0.5f, 0.5f);
                    PopupRect.pivot = new Vector2(0.5f, 0.5f);
                    break;
            }
        }
        protected virtual void Show()
        {
            PopupRect.transform.localScale = Vector3.one;
            PopupRect.SetLeft(0);
            PopupRect.SetRight(0);
            UpdateSizes();
            InitToolbar();
            Utils.RunAsync(() =>
            {
                StartOpenAnimation();
            });
        }
        protected virtual void InitAnimationParameters()
        {
            if (PopupRect == null)
                return;
            Utils.ForceUpdateLayout(PopupRect);

            var rect = PopupRect.rect;
            AnimationStartPos = Alignment switch
            {
                PopupAlignment.Left => new Vector2(-rect.width, 0),
                PopupAlignment.Right => new Vector2(rect.width, 0),
                PopupAlignment.Top => new Vector2(0, rect.height),
                PopupAlignment.Bottom => new Vector2(0, -rect.height),
                PopupAlignment.Center => new Vector2(0, 0),
                _ => AnimationStartPos
            };
            AnimationEndPos = Vector2.zero;
        }
        private void StartCloseAnimation()
        {
            if (PopupRect == null)
                return;

            IsClosing = true;
            InitAnimationParameters();

            PopupRect.DOAnchorPos(AnimationStartPos, CloseDuration).From(AnimationEndPos)
                .OnComplete(AfterClose);
        }
        private void StartOpenAnimation()
        {
            if (PopupRect == null)
                return;

            InitAnimationParameters();

            PopupRect.DOAnchorPos(AnimationEndPos, OpenDuration).From(AnimationStartPos)
                .OnComplete(AfterOpen);
        }
        protected virtual void AfterClose()
        {
            DestroyPopup();
        }
        protected virtual void AfterOpen()
        {
        }
        public virtual void DestroyPopup()
        {
            if (_background != null)
            {
                _background.Dispose();
                _background = null;
            }
            if (PopupRect != null)
            {
                PopupsManager.instance.RemovePopup(this);
                UnityEngine.Object.Destroy(PopupRect.gameObject);
                PopupRect = null;
            }

            if (TouchRotation.Instance != null)
                TouchRotation.Instance.CanTouchRotate = true;
        }
        public virtual void Dispose()
        {
            if (!IsClosing)
                StartCloseAnimation();
            if (_background != null)
            {
                _background.Dispose();
                _background = null;
            }
        }
    }
}

public enum PopupAlignment
{
    Bottom,
    Top,
    Left,
    Right,
    Center
}