using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    internal sealed class LottieAnimationsPreview : MonoBehaviour, System.IDisposable
    {
        [SerializeField] private AnimationPreview _animationPreviewPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _scrollRectViewPort;
        [SerializeField] private RectTransform _scrollRectContent;
        [SerializeField] private int _columns;
        [SerializeField] private int _gabBetweenItems;

        private List<AnimationPreview> _animationPreviews;

        internal void Init(Data.LottieAnimations lottieAnimations)
        {
            string[] animations = lottieAnimations.Animations;
            _animationPreviews = new List<AnimationPreview>(animations.Length);
            
            Vector2 viewPortSize = _scrollRectViewPort.rect.size;
            float oneItemSize = (viewPortSize.x / _columns) - (_gabBetweenItems * _columns);
            for (int i = 0; i < animations.Length; ++i)
            {
                string animation = animations[i];
                AnimationPreview animationPreview = Instantiate(_animationPreviewPrefab, _scrollRectContent);
                animationPreview.Init(animation, 128, 128);
                animationPreview.RectTransform.anchoredPosition = new Vector3(
                    i % _columns * oneItemSize + _gabBetweenItems,
                    -i / _columns * oneItemSize - _gabBetweenItems);
                animationPreview.RectTransform.sizeDelta = new Vector2(oneItemSize, oneItemSize);
                _animationPreviews.Add(animationPreview);
            }
            _scrollRectContent.sizeDelta = new Vector2(
                _scrollRectContent.sizeDelta.x,
                (animations.Length / _columns * oneItemSize) + (animations.Length / _columns * _gabBetweenItems));
        }
        public void Dispose()
        {
            for (int i = 0; i < _animationPreviews.Count; ++i)
            {
                _animationPreviews[i].Dispose();
            }
            _animationPreviews.Clear();
        }
        private void Update()
        {
            for (int i = 0; i < _animationPreviews.Count; ++i)
            {
                _animationPreviews[i].DoUpdateAsync();
            }
        }
        private void LateUpdate()
        {
            for (int i = 0; i < _animationPreviews.Count; ++i)
            {
                _animationPreviews[i].DoDrawOneFrameAsyncGetResult();
            }
        }
    }
}
