using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PlanaOutputText : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float moveDistance = 50f;  // How far up the text moves
    [SerializeField] private float baseDisplayDuration = 2f;  // Base duration for short sentences
    [SerializeField] private float charactersPerSecond = 15f;  // Additional time per character

    [Header("Speaker Settings")]
    [SerializeField] private string speakerPrefix = "<color=#03fcf0>«×«é«Ê£º</color>";

    [SerializeField] private PlanaFaceController faceController;

    private Queue<string> sentenceQueue = new Queue<string>();
    private bool isDisplaying = false;
    private Vector2 startPosition;

    private void Start() {
        startPosition = textDisplay.rectTransform.anchoredPosition;
        canvasGroup.alpha = 0f;

        //PlanaQueueText("ìíÜâåÞªÇªâªÇª­ªëªÎª««È«é«¤ª·ªÞª¹¡£ªï£¡ªÇª­ªÞª¹ªÍ£¿ª¹ª´ª¤ªÇª¹¡£");


    }

    public void PlanaQueueText(string text) {
        // Split text using both Japanese and English sentence endings
        // Matches: 
        // - English .!? followed by whitespace
        // - Japanese ¡££¡£¿ followed by optional whitespace
        string[] sentences = Regex.Split(text, @"(?<=[.!?¡££¡£¿])\s*");

        foreach (string sentence in sentences) {
            string trimmedSentence = sentence.Trim();
            if (!string.IsNullOrWhiteSpace(trimmedSentence)) {
                sentenceQueue.Enqueue(trimmedSentence);
            }
        }

        if (!isDisplaying) {
            StartCoroutine(DisplayNextSentence());
        }
    }


    private IEnumerator DisplayNextSentence() {
        isDisplaying = true;

        while (sentenceQueue.Count > 0) {
            string currentSentence = sentenceQueue.Dequeue();
            // Add speaker prefix to the sentence
            textDisplay.text = speakerPrefix + currentSentence;
            faceController.StartSpeaking();
            // Adjust display duration for Japanese characters
            // Note: Don't include the prefix HTML tags in duration calculation
            float displayDuration = CalculateDisplayDuration(currentSentence);

            // Rest of the code remains the same...
            textDisplay.rectTransform.anchoredPosition = startPosition;
            yield return StartCoroutine(AnimateText(true));
            yield return new WaitForSeconds(displayDuration);
            faceController.StopSpeaking();
            yield return StartCoroutine(AnimateText(false));
        }

        isDisplaying = false;
    }

    private float CalculateDisplayDuration(string text) {
        // Count Japanese characters (hiragana, katakana, kanji)
        int japaneseCharCount = 0;
        foreach (char c in text) {
            if ((c >= 0x3040 && c <= 0x309F) ||    // Hiragana
                (c >= 0x30A0 && c <= 0x30FF) ||    // Katakana
                (c >= 0x4E00 && c <= 0x9FFF))      // Common Kanji
            {
                japaneseCharCount++;
            }
        }

        // Japanese characters typically need more time to read
        float japaneseCharTime = japaneseCharCount / (charactersPerSecond * 0.6f); // Slow down for Japanese
        float englishCharTime = (text.Length - japaneseCharCount) / charactersPerSecond;

        return baseDisplayDuration + japaneseCharTime + englishCharTime;
    }

    private IEnumerator AnimateText(bool fadeIn) {
        float duration = fadeIn ? fadeInDuration : fadeOutDuration;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        Vector2 startPos = fadeIn ? startPosition : textDisplay.rectTransform.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, moveDistance);
        float elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Update alpha
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            // Update position
            textDisplay.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            yield return null;
        }

        // Ensure we reach the final values
        canvasGroup.alpha = endAlpha;
        textDisplay.rectTransform.anchoredPosition = endPos;
    }
}