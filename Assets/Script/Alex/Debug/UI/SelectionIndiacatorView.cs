using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;


public class SelectionIndicatorView : MonoBehaviour {
    [SerializeField] private Image selectionFrame;

    [Header("Animation")]
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private float animationDuration = 0.4f;

    private Tween _animationTween;

    public void Show() {
        selectionFrame.gameObject.SetActive(true);
        StartAnimation();
    }

    public void Hide() {
        StopAnimation();
        selectionFrame.gameObject.SetActive(false);
    }

    private void StartAnimation() {
        StopAnimation();

        selectionFrame.transform.localScale = Vector3.one;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(
            selectionFrame.transform
                .DOScale(scaleMultiplier, animationDuration)
        );

        sequence.Join(
            selectionFrame
                .DOFade(0.5f, animationDuration)
        );

        sequence.SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

        _animationTween = sequence;
    }

    private void StopAnimation() {
        if (_animationTween != null) {
            _animationTween.Kill();
            _animationTween = null;
        }

        selectionFrame.transform.localScale = Vector3.one;
    }

    private void OnDestroy() {
        StopAnimation();
    }
}