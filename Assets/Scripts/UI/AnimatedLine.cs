using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class AnimatedLine : MonoBehaviour {
    [SerializeField] private float expandDuration = 0.5f;
    [SerializeField] private Color lineColor = Color.cyan;

    private RectTransform rectTransform;
    private Image lineImage;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        lineImage = GetComponent<Image>();
        lineImage.color = lineColor;
        rectTransform.localScale = new Vector3(0, 1, 1);
    }

    public async Task Expand() {
        await rectTransform.DOScale(new Vector3(1, 1, 1), expandDuration)
            .SetEase(Ease.OutExpo).AsyncWaitForCompletion();
    }

    public async Task Collapse() {
        await rectTransform.DOScale(new Vector3(0, 1, 1), expandDuration * 0.5f)
            .SetEase(Ease.InExpo).AsyncWaitForCompletion();
    }
}