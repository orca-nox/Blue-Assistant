using UnityEngine;
using UnityEngine.UI;

public class FancyLoadingSpinner : MonoBehaviour {
    private Image loadingImage;
    public float fillSpeed = 200f;

    private void Start() {
        loadingImage = GetComponent<Image>();
        loadingImage.type = Image.Type.Filled;
        loadingImage.fillMethod = Image.FillMethod.Radial360;
        loadingImage.fillOrigin = (int)Image.Origin360.Top;
    }

    private void OnEnable() {
        loadingImage.fillAmount = 0f;
    }

    private void Update() {
        loadingImage.fillAmount = (loadingImage.fillAmount + (fillSpeed * Time.deltaTime / 360f)) % 1f;
    }
}