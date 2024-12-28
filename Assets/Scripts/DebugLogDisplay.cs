using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DebugLogDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI debugText; // Reference to your UI text component
    [SerializeField] private int maxMessages = 25; // Maximum number of messages to show
    [SerializeField] private GameObject logPanel; // Reference to the panel containing the log
    private Queue<string> messages = new Queue<string>();
    private bool isInitialized = false;

    void Start() {
        // Hide the log panel initially
        logPanel.SetActive(false);
    }

    void Update() {
        // Check for tilde key press (backquote)
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            ToggleLogPanel();
        }
    }

    void ToggleLogPanel() {
        logPanel.SetActive(!logPanel.activeSelf);
    }

    void OnEnable() {
        if (!isInitialized) {
            Application.logMessageReceived += HandleLog;
            isInitialized = true;
        }
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
        isInitialized = false;
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        string message = "";

        switch (type) {
            case LogType.Error:
                message = $"<color=red>[ERROR] {logString}</color>";
                break;
            case LogType.Assert:
                message = $"<color=red>[ASSERT] {logString}</color>";
                break;
            case LogType.Warning:
                message = $"<color=yellow>[WARNING] {logString}</color>";
                break;
            case LogType.Log:
                message = $"[LOG] {logString}";
                break;
            case LogType.Exception:
                message = $"<color=red>[EXCEPTION] {logString}\n{stackTrace}</color>";
                break;
        }

        message = $"[{System.DateTime.Now.ToString("HH:mm:ss")}] {message}";
        messages.Enqueue(message);

        while (messages.Count > maxMessages) {
            messages.Dequeue();
        }

        UpdateUI();
    }

    void UpdateUI() {
        if (debugText != null) {
            debugText.text = string.Join("\n", messages.ToArray());
        }
    }

    public void ClearLog() {
        messages.Clear();
        UpdateUI();
    }
}