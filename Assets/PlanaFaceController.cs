using UnityEngine;
using System.Collections;

public class PlanaFaceController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer faceMesh;
    [SerializeField] private PlanaOutputText outputText; // To check if speaking

    [Header("Mouth Settings")]
    [SerializeField] private float mouthMoveSpeed = 8f;
    [SerializeField] private float mouthOpenAmount = 100f;
    [SerializeField] private AnimationCurve mouthMovement = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Blinking Settings")]
    [SerializeField] private float minBlinkInterval = 2f;
    [SerializeField] private float maxBlinkInterval = 6f;
    [SerializeField] private float blinkDuration = 0.15f;

    private bool isSpeaking = false;
    private Coroutine mouthCoroutine;
    private Coroutine blinkCoroutine;

    private void Start() {
        StartCoroutine(BlinkRoutine());
    }

    public void StartSpeaking() {
        isSpeaking = true;
        if (mouthCoroutine != null)
            StopCoroutine(mouthCoroutine);
        mouthCoroutine = StartCoroutine(MouthMovementRoutine());
    }

    public void StopSpeaking() {
        isSpeaking = false;
        if (mouthCoroutine != null) {
            StopCoroutine(mouthCoroutine);
            faceMesh.SetBlendShapeWeight(0, 0); // Reset mouth
        }
    }

    private IEnumerator MouthMovementRoutine() {
        float time = 0;

        while (isSpeaking) {
            time += Time.deltaTime * mouthMoveSpeed;
            // Create natural oscillation using sine wave and animation curve
            float mouthValue = mouthMovement.Evaluate((Mathf.Sin(time) + 1f) * 0.5f) * mouthOpenAmount;
            faceMesh.SetBlendShapeWeight(0, mouthValue);

            yield return null;
        }
    }

    private IEnumerator BlinkRoutine() {
        while (true) {
            // Wait for random interval
            yield return new WaitForSeconds(Random.Range(minBlinkInterval, maxBlinkInterval));

            // Blink animation
            float elapsed = 0;
            // Close eyes
            while (elapsed < blinkDuration / 2) {
                elapsed += Time.deltaTime;
                float t = elapsed / (blinkDuration / 2);
                faceMesh.SetBlendShapeWeight(5, Mathf.Lerp(0, 100, t));
                yield return null;
            }

            // Open eyes
            elapsed = 0;
            while (elapsed < blinkDuration / 2) {
                elapsed += Time.deltaTime;
                float t = elapsed / (blinkDuration / 2);
                faceMesh.SetBlendShapeWeight(5, Mathf.Lerp(100, 0, t));
                yield return null;
            }
        }
    }
}
