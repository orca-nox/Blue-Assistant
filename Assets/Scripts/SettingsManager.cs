// SettingsManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class SettingsManager : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private Image headerLine;         // Changed to Image component
    [SerializeField] private RectTransform titleText;
    [SerializeField] private RectTransform settingsContainer;
    [SerializeField] private Button returnButton;

    [Header("Settings UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;

    [Header("Animation Settings")]
    [SerializeField] private float lineExpandDuration = 0.5f;
    [SerializeField] private float titlePopDuration = 0.3f;
    [SerializeField] private float settingsPopDuration = 0.4f;
    [SerializeField] private float elementPopDelay = 0.05f;

    private RectTransform lineRect;  // Reference to the line's RectTransform
    private Vector2 lineOriginalSize;

    private const string RESOLUTION_KEY = "Resolution";
    private const string TEXT_SPEED_KEY = "TextSpeed";
    private const string MOUSE_SENSITIVITY_KEY = "MouseSensitivity";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string VOICE_VOLUME_KEY = "VoiceVolume";

    private void OnEnable() {
        Debug.Log("SettingsManager OnEnable called");
        _ = PlayIntroAnimation(); // Fire and forget since OnEnable can't be async
    }

    private void Awake() {
        // Get the RectTransform for the line
        if (headerLine != null) {
            lineRect = headerLine.GetComponent<RectTransform>();
            lineOriginalSize = lineRect.sizeDelta;
            // Start with zero width but full height
            lineRect.sizeDelta = new Vector2(0, lineOriginalSize.y);
        }

        // Hide title and settings container initially
        if (titleText != null) {
            titleText.gameObject.SetActive(false);
        }

        if (settingsContainer != null) {
            settingsContainer.gameObject.SetActive(false);
        }

        AddSettingsListeners();
    }

    private void AddSettingsListeners() {
        resolutionDropdown?.onValueChanged.AddListener(_ => SaveSettings());
        textSpeedSlider?.onValueChanged.AddListener(_ => SaveSettings());
        mouseSensitivitySlider?.onValueChanged.AddListener(_ => SaveSettings());
        bgmVolumeSlider?.onValueChanged.AddListener(_ => SaveSettings());
        sfxVolumeSlider?.onValueChanged.AddListener(_ => SaveSettings());
        voiceVolumeSlider?.onValueChanged.AddListener(_ => SaveSettings());
    }

    public void LoadSettings() {
        if (resolutionDropdown != null)
            resolutionDropdown.value = PlayerPrefs.GetInt(RESOLUTION_KEY, 0);

        if (textSpeedSlider != null)
            textSpeedSlider.value = PlayerPrefs.GetFloat(TEXT_SPEED_KEY, 1.0f);

        if (mouseSensitivitySlider != null)
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_KEY, 0.5f);

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.value = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1.0f);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);

        if (voiceVolumeSlider != null)
            voiceVolumeSlider.value = PlayerPrefs.GetFloat(VOICE_VOLUME_KEY, 1.0f);
    }

    public void SaveSettings() {
        if (resolutionDropdown != null)
            PlayerPrefs.SetInt(RESOLUTION_KEY, resolutionDropdown.value);

        if (textSpeedSlider != null)
            PlayerPrefs.SetFloat(TEXT_SPEED_KEY, textSpeedSlider.value);

        if (mouseSensitivitySlider != null)
            PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, mouseSensitivitySlider.value);

        if (bgmVolumeSlider != null)
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolumeSlider.value);

        if (sfxVolumeSlider != null)
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolumeSlider.value);

        if (voiceVolumeSlider != null)
            PlayerPrefs.SetFloat(VOICE_VOLUME_KEY, voiceVolumeSlider.value);

        PlayerPrefs.Save();
        ApplySettings();
    }

    private void ApplySettings() {
        // Implement settings application logic here
        Debug.Log("Settings applied");
    }

    public async Task PlayIntroAnimation() {
        Debug.Log("Starting intro animation");

        // Reset initial states
        if (lineRect != null) {
            lineRect.sizeDelta = new Vector2(0, lineOriginalSize.y);
            headerLine.gameObject.SetActive(true);
        }

        if (titleText != null) {
            titleText.gameObject.SetActive(true);
            titleText.localScale = Vector3.zero;
        }

        if (settingsContainer != null) {
            settingsContainer.gameObject.SetActive(true);
            settingsContainer.localScale = Vector3.zero;
        }

        // Animate the line width
        if (lineRect != null) {
            await DOTween.To(() => lineRect.sizeDelta.x,
                            x => lineRect.sizeDelta = new Vector2(x, lineOriginalSize.y),
                            lineOriginalSize.x,
                            lineExpandDuration)
                    .SetEase(Ease.OutExpo)
                    .AsyncWaitForCompletion();

            Debug.Log("Line animation completed");
        }

        // Animate title pop
        if (titleText != null) {
            titleText.DOScale(Vector3.one, titlePopDuration)
                    .SetEase(Ease.OutBack);

            Debug.Log("Title animation started");
            await Task.Delay((int)(titlePopDuration * 1000));
        }

        // Animate settings container
        if (settingsContainer != null) {
            settingsContainer.DOScale(Vector3.one, settingsPopDuration)
                           .SetEase(Ease.OutBack);

            Debug.Log("Settings container animation started");
            await Task.Delay((int)(settingsPopDuration * 1000));
        }

        Debug.Log("Intro animation completed");
    }

    public async Task PlayOutroAnimation() {
        Debug.Log("Starting outro animation");

        // Collapse settings first
        if (settingsContainer != null) {
            settingsContainer.DOScale(Vector3.zero, settingsPopDuration * 0.5f)
                           .SetEase(Ease.InBack);
            await Task.Delay((int)(settingsPopDuration * 0.5f * 1000));
        }

        // Collapse title
        if (titleText != null) {
            titleText.DOScale(Vector3.zero, titlePopDuration * 0.5f)
                    .SetEase(Ease.InBack);
            await Task.Delay((int)(titlePopDuration * 0.5f * 1000));
        }

        // Shrink line
        if (lineRect != null) {
            await DOTween.To(() => lineRect.sizeDelta.x,
                            x => lineRect.sizeDelta = new Vector2(x, lineOriginalSize.y),
                            0f,
                            lineExpandDuration * 0.5f)
                    .SetEase(Ease.InExpo)
                    .AsyncWaitForCompletion();
        }

        // Deactivate all elements
        settingsContainer?.gameObject.SetActive(false);
        titleText?.gameObject.SetActive(false);
        headerLine?.gameObject.SetActive(false);

        Debug.Log("Outro animation completed");
    }
}