using UnityEngine;
using TMPro;
using System.Collections;

public class InputBoxManager : MonoBehaviour {
    [SerializeField] private CanvasGroup inputBoxCanvasGroup;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private float fadeDuration = 0.5f;

    private bool isVisible = false;
    private float currentAlpha = 0f;

    public bool IsInputActive => isVisible && inputField.isFocused;
    public static InputBoxManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void FadeIn() {
        isVisible = true;
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f));
        inputField.ActivateInputField();
    }

    private void FadeOut() {
        isVisible = false;
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float targetAlpha) {
        float startAlpha = inputBoxCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            inputBoxCanvasGroup.alpha = newAlpha;
            yield return null;
        }

        inputBoxCanvasGroup.alpha = targetAlpha;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (!isVisible) {
                FadeIn();
            } else if (!string.IsNullOrWhiteSpace(inputField.text)) {
                ChatGPTManager.Instance.RequestChatGPT(inputField.text);
                inputField.text = "";
                FadeOut();
            } else {
                FadeOut();
            }
        }
    }

}