using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MenuAnimation : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RectTransform titlePanel;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private RectTransform separatorLine;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private MenuManager menuManager;

    [Header("Entry Animation Settings")]
    [SerializeField] private float entryLineExpandDuration = 0.5f;
    [SerializeField] private float entryTitlePanelDuration = 0.4f;
    [SerializeField] private float entryTitlePanelDelay = 0.2f;
    [SerializeField] private float entryTitlePanelDistance = 100f;
    [SerializeField] private float entryContentPanelDuration = 0.4f;
    [SerializeField] private float entryContentPanelDelay = 0f;
    [SerializeField] private float entryContentPanelDistance = 1000f;

    [Header("Exit Animation Settings")]
    [SerializeField] private float exitLineExpandDuration = 0.5f;
    [SerializeField] private float exitTitlePanelDuration = 0.4f;
    [SerializeField] private float exitTitlePanelDelay = 0f;
    [SerializeField] private float exitTitlePanelDistance = 100f;
    [SerializeField] private float exitContentPanelDuration = 0.4f;
    [SerializeField] private float exitContentPanelDelay = 0f;
    [SerializeField] private float exitContentPanelDistance = 1000f;

    [Header("Animation Style")]
    [SerializeField] private Ease easeType = Ease.OutExpo;

    private Vector2 titleStartPosition;
    private Vector2 contentStartPosition;
    private Vector2 separatorStartScale;

    private void Awake() {
        // Store initial positions and scales
        titleStartPosition = titlePanel.anchoredPosition;
        contentStartPosition = contentPanel.anchoredPosition;
        separatorStartScale = separatorLine.localScale;

        // Setup return button
        returnToMainMenuButton.onClick.AddListener(OnReturnButtonClick);

        // Initialize panels
        titlePanel.gameObject.SetActive(false);
        contentPanel.gameObject.SetActive(false);
        separatorLine.localScale = new Vector2(0, separatorStartScale.y);
    }

    private void OnEnable() {
        PlayEntryAnimation();
    }

    private void PlayEntryAnimation() {
        // Reset positions
        titlePanel.anchoredPosition = titleStartPosition + Vector2.down * entryTitlePanelDistance;
        contentPanel.anchoredPosition = contentStartPosition + Vector2.up * entryContentPanelDistance;
        separatorLine.localScale = new Vector2(0, separatorStartScale.y);

        // Enable objects
        titlePanel.gameObject.SetActive(true);
        contentPanel.gameObject.SetActive(true);

        // Animate separator line
        separatorLine.DOScaleX(separatorStartScale.x, entryLineExpandDuration)
            .SetEase(easeType);

        // Animate content panel
        contentPanel.DOAnchorPos(contentStartPosition, entryContentPanelDuration)
            .SetEase(easeType)
            .SetDelay(entryContentPanelDelay);

        // Animate title panel
        titlePanel.DOAnchorPos(titleStartPosition, entryTitlePanelDuration)
            .SetEase(easeType)
            .SetDelay(entryTitlePanelDelay);
    }

    private void OnReturnButtonClick() {
        StartCoroutine(PlayExitAnimation());
    }

    private IEnumerator PlayExitAnimation() {
        // Calculate the maximum panel animation duration including delays
        float maxPanelDuration = Mathf.Max(
            exitTitlePanelDuration + exitTitlePanelDelay,
            exitContentPanelDuration + exitContentPanelDelay
        );

        // Animate title panel
        titlePanel.DOAnchorPos(
            titleStartPosition + Vector2.down * exitTitlePanelDistance,
            exitTitlePanelDuration
        )
            .SetEase(easeType)
            .SetDelay(exitTitlePanelDelay);

        // Animate content panel
        contentPanel.DOAnchorPos(
            contentStartPosition + Vector2.up * exitContentPanelDistance,
            exitContentPanelDuration
        )
            .SetEase(easeType)
            .SetDelay(exitContentPanelDelay);

        // Wait for the longest panel animation to complete
        yield return new WaitForSeconds(maxPanelDuration*0.5f);

        // Animate separator line
        separatorLine.DOScaleX(0, exitLineExpandDuration)
            .SetEase(easeType);



        // Call menu manager and disable panel
        menuManager.OnReturnToMainMenuClick();
        yield return new WaitForSeconds(exitLineExpandDuration);
        gameObject.SetActive(false);

    }

    private void OnDisable() {
        // Kill all tweens to prevent animation conflicts
        DOTween.Kill(titlePanel);
        DOTween.Kill(contentPanel);
        DOTween.Kill(separatorLine);
    }
}