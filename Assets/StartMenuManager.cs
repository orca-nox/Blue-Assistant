using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections;

public class StartMenuManager : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private GameObject apiKeyPanel;
    [SerializeField] private TMP_InputField apiKeyInput;
    [SerializeField] private TMP_InputField orgKeyInput;
    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Dependencies")]
    [SerializeField] private ChatGPTManager chatGPTManager;

    [Header("Settings")]
    [SerializeField] private float fadeTime = 1f;

    private void Start() {
        // Initially hide API key panel and error text
        apiKeyPanel.SetActive(false);
        errorText.gameObject.SetActive(false);

        // Load saved keys if they exist
        LoadSavedKeys();
    }

    public void OnStartGamePressed() {
        // Check if API key exists
        if (string.IsNullOrEmpty(chatGPTManager.apiKey) || string.IsNullOrEmpty(chatGPTManager.orgKey)) {
            ShowError("Please enter an API Key first!");
            return;
        }

        // Start fade out animation
        StartCoroutine(FadeOutMenu());
    }

    public void OnAPIKeyButtonPressed() {
        apiKeyPanel.SetActive(true);

        // Pre-fill fields if keys exist
        if (!string.IsNullOrEmpty(chatGPTManager.apiKey))
            apiKeyInput.text = chatGPTManager.apiKey;
        if (!string.IsNullOrEmpty(chatGPTManager.orgKey))
            orgKeyInput.text = chatGPTManager.orgKey;
    }

    public void OnSaveAPIKeys() {
        string apiKey = apiKeyInput.text.Trim();
        string orgKey = orgKeyInput.text.Trim();

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(orgKey)) {
            ShowError("Both API Key and Organization Key are required!");
            return;
        }

        // Save to files
        SaveKeys(apiKey, orgKey);

        // Update ChatGPTManager
        chatGPTManager.apiKey = apiKey;
        chatGPTManager.orgKey = orgKey;

        // Hide panel
        apiKeyPanel.SetActive(false);
        errorText.gameObject.SetActive(false);
    }

    public void OnExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void SaveKeys(string apiKey, string orgKey) {
        string persistentPath = Application.persistentDataPath;

        File.WriteAllText(Path.Combine(persistentPath, "apikey.txt"), apiKey);
        File.WriteAllText(Path.Combine(persistentPath, "orgkey.txt"), orgKey);
    }

    private void LoadSavedKeys() {
        string persistentPath = Application.persistentDataPath;
        string apiKeyPath = Path.Combine(persistentPath, "apikey.txt");
        string orgKeyPath = Path.Combine(persistentPath, "orgkey.txt");

        try {
            if (File.Exists(apiKeyPath))
                chatGPTManager.apiKey = File.ReadAllText(apiKeyPath);

            if (File.Exists(orgKeyPath))
                chatGPTManager.orgKey = File.ReadAllText(orgKeyPath);
        } catch (System.Exception e) {
            Debug.LogError($"Error loading saved keys: {e.Message}");
        }
    }

    private void ShowError(string message) {
        errorText.text = message;
        errorText.gameObject.SetActive(true);

        // Hide error after 3 seconds
        StartCoroutine(HideErrorAfterDelay());
    }

    private IEnumerator HideErrorAfterDelay() {
        yield return new WaitForSeconds(3f);
        errorText.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutMenu() {
        float elapsedTime = 0f;
        float startAlpha = mainMenuCanvas.alpha;

        while (elapsedTime < fadeTime) {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeTime);
            mainMenuCanvas.alpha = newAlpha;
            yield return null;
        }

        mainMenuCanvas.gameObject.SetActive(false);
    }
}