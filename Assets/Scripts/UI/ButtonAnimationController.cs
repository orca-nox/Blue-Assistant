using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private float hoverOffset = 20f;
    [SerializeField] private float clickOffset = 10f;
    [SerializeField] private float animationDuration = 0.2f;

    private RectTransform rectTransform;
    private float startingX;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        startingX = rectTransform.anchoredPosition.x;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // Move right from the current position
        rectTransform.DOAnchorPosX(startingX + hoverOffset, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Return to original position
        rectTransform.DOAnchorPosX(startingX, animationDuration);
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Slight move right when clicked
        rectTransform.DOAnchorPosX(startingX + clickOffset, animationDuration * 0.5f);
    }

    public void OnPointerUp(PointerEventData eventData) {
        // Return to hover position if still hovering
        rectTransform.DOAnchorPosX(startingX + hoverOffset, animationDuration * 0.5f);
    }

    // Optional: If you need to update the starting position after moving the button
    public void UpdateStartingPosition() {
        startingX = rectTransform.anchoredPosition.x;
    }

#if UNITY_EDITOR
    // This helps update the starting position when you move the button in the editor
    private void OnValidate() {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        startingX = rectTransform.anchoredPosition.x;
    }
#endif
}