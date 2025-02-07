// AnimatedParallelogram.cs
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class AnimatedParallelogram : MonoBehaviour {
    [SerializeField] private float slideDistance = 1000f;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private int orderIndex = 0;
    [SerializeField] private AnimationDirection slideDirection = AnimationDirection.Horizontal;

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    public enum AnimationDirection {
        Horizontal,
        Vertical
    }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void SlideOut() {
        float delay = orderIndex * 0.1f;
        if (slideDirection == AnimationDirection.Horizontal) {
            rectTransform.DOAnchorPosX(originalPosition.x + slideDistance, animationDuration)
                .SetDelay(delay)
                .SetEase(Ease.InOutQuad);
        } else {
            rectTransform.DOAnchorPosY(originalPosition.y - slideDistance, animationDuration)
                .SetDelay(delay)
                .SetEase(Ease.InOutQuad);
        }
    }

    public void SlideIn() {
        // Set starting position
        if (slideDirection == AnimationDirection.Horizontal) {
            rectTransform.anchoredPosition = new Vector2(originalPosition.x + slideDistance, originalPosition.y);
        } else {
            rectTransform.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y + slideDistance);
        }

        float delay = orderIndex * 0.1f;
        if (slideDirection == AnimationDirection.Horizontal) {
            rectTransform.DOAnchorPosX(originalPosition.x, animationDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutQuad);
        } else {
            rectTransform.DOAnchorPosY(originalPosition.y, animationDuration)
                .SetDelay(delay)
                .SetEase(Ease.OutQuad);
        }
    }

    public void ResetPosition() {
        rectTransform.anchoredPosition = originalPosition;
    }
}