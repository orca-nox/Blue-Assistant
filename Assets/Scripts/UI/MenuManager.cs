// MenuManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour {
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject teacherDataPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Teacher Data Fields")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField apiKeyInput;
    [SerializeField] private TMP_Dropdown aiServiceDropdown;

    [Header("Settings")]
    [SerializeField] private Slider resolutionSlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private Slider volumeSlider;

    [Header("Animation Settings")]
    [SerializeField] private float transitionDelay = 0.2f;

    public bool IsMenuActive { get; private set; }
    private GameObject currentPanel;
    private bool isAnimating = false;

    private async void Start() {
        currentPanel = mainMenuPanel;
        await ShowMainMenu();
        LoadSettings();
        LoadTeacherData();
    }

    private void LoadTeacherData() {
        // Load name - default to "เป฿ๆ" if not set
        string savedName = PlayerPrefs.GetString("TeacherName", "เป฿ๆ");
        if (nameInput != null) {
            nameInput.text = savedName;
        }

        // Load API key
        string savedApiKey = PlayerPrefs.GetString("TeacherApiKey", "");
        if (apiKeyInput != null) {
            apiKeyInput.text = savedApiKey;
        }

        // Load selected AI service
        int savedServiceIndex = PlayerPrefs.GetInt("AIService", 0);
        if (aiServiceDropdown != null) {
            aiServiceDropdown.value = savedServiceIndex;
        }

        // Add corresponding save methods
        if (nameInput != null) {
            nameInput.onEndEdit.AddListener((string value) => {
                PlayerPrefs.SetString("TeacherName", value);
                PlayerPrefs.Save();
            });
        }

        if (apiKeyInput != null) {
            apiKeyInput.onEndEdit.AddListener((string value) => {
                PlayerPrefs.SetString("TeacherApiKey", value);
                PlayerPrefs.Save();
            });
        }

        if (aiServiceDropdown != null) {
            aiServiceDropdown.onValueChanged.AddListener((int value) => {
                PlayerPrefs.SetInt("AIService", value);
                PlayerPrefs.Save();
            });
        }
    }

    public async Task ShowMainMenu() {
        if (isAnimating) return;
        isAnimating = true;

        if (currentPanel != null && currentPanel != mainMenuPanel) {
            await HideCurrentPanel(currentPanel);
            await Task.Delay((int)(transitionDelay * 1000));
        }

        mainMenuPanel.SetActive(true);
        var buttons = mainMenuPanel.GetComponentsInChildren<AnimatedParallelogram>(true);
        foreach (var button in buttons) {
            button.ResetPosition();
            button.SlideIn();
        }

        currentPanel = mainMenuPanel;
        IsMenuActive = true;
        await Task.Delay((int)(buttons.Length * 100 + 500));
        isAnimating = false;
    }

    public async Task ShowSettings() {
        if (isAnimating) return;
        isAnimating = true;

        if (currentPanel != null) {
            await HideCurrentPanel(currentPanel);
            await Task.Delay((int)(transitionDelay * 1000));
        }

        settingsPanel.SetActive(true);
        var elements = settingsPanel.GetComponentsInChildren<AnimatedParallelogram>();
        foreach (var element in elements) {
            element.ResetPosition();
            element.SlideIn();
        }

        currentPanel = settingsPanel;
        IsMenuActive = true;
        await Task.Delay((int)(elements.Length * 100 + 500));
        isAnimating = false;
    }

    public async Task ShowTeacherData() {
        if (isAnimating) return;
        isAnimating = true;

        if (currentPanel != null) {
            await HideCurrentPanel(currentPanel);
            await Task.Delay((int)(transitionDelay * 1000));
        }

        teacherDataPanel.SetActive(true);
        var elements = teacherDataPanel.GetComponentsInChildren<AnimatedParallelogram>();
        foreach (var element in elements) {
            element.ResetPosition();
            element.SlideIn();
        }

        currentPanel = teacherDataPanel;
        IsMenuActive = true;
        await Task.Delay((int)(elements.Length * 100 + 500));
        isAnimating = false;
    }

    public async Task ShowCredits() {
        if (isAnimating) return;
        isAnimating = true;

        if (currentPanel != null) {
            await HideCurrentPanel(currentPanel);
            await Task.Delay((int)(transitionDelay * 1000));
        }

        creditsPanel.SetActive(true);
        var elements = creditsPanel.GetComponentsInChildren<AnimatedParallelogram>();
        foreach (var element in elements) {
            element.ResetPosition();
            element.SlideIn();
        }

        currentPanel = creditsPanel;
        IsMenuActive = true;
        await Task.Delay((int)(elements.Length * 100 + 500));
        isAnimating = false;
    }

    private async Task HideCurrentPanel(GameObject panel) {
        var elements = panel.GetComponentsInChildren<AnimatedParallelogram>();
        foreach (var element in elements) {
            element.SlideOut();
        }
        await Task.Delay((int)(elements.Length * 100 + 500));
        panel.SetActive(false);
    }

    public async Task ReturnToMainMenu() {
        await ShowMainMenu();
    }

    public async Task ValidateApiKey() {
        string apiKey = apiKeyInput.text;
        if (string.IsNullOrEmpty(apiKey)) {
            ShowError("Error: NNO - API Key not found");
            return;
        }

        bool isValid = await TestApiKey(apiKey);
        if (!isValid) {
            ShowError("Error: Invalid API Key");
        }
    }

    private async Task<bool> TestApiKey(string apiKey) {
        // Simulated API check
        await Task.Delay(1000);
        return !string.IsNullOrEmpty(apiKey);
    }

    private void ShowError(string message) {
        Debug.LogError(message);
        // Implement error UI display logic here
    }

    private void SaveSettings() {
        PlayerPrefs.SetFloat("Resolution", resolutionSlider.value);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivitySlider.value);
        PlayerPrefs.SetFloat("TextSpeed", textSpeedSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadSettings() {
        /*
        resolutionSlider.value = PlayerPrefs.GetFloat("Resolution", 1.0f);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        textSpeedSlider.value = PlayerPrefs.GetFloat("TextSpeed", 1.0f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);

        // Add listeners for saving settings
        resolutionSlider.onValueChanged.AddListener((float value) => SaveSettings());
        mouseSensitivitySlider.onValueChanged.AddListener((float value) => SaveSettings());
        textSpeedSlider.onValueChanged.AddListener((float value) => SaveSettings());
        volumeSlider.onValueChanged.AddListener((float value) => SaveSettings());
        */
    }

    // Button click wrapper methods for Unity UI
    public void OnMainMenuButtonClick() {
        _ = ShowMainMenu();
    }

    public void OnSettingsButtonClick() {
        _ = ShowSettings();
    }

    public void OnTeacherDataButtonClick() {
        _ = ShowTeacherData();
    }

    public void OnCreditsButtonClick() {
        _ = ShowCredits();
    }

    public void OnReturnToMainMenuClick() {
        _ = ReturnToMainMenu();
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
