using UnityEngine;
using System.IO;

public class SettingsParser : MonoBehaviour {
    private string apiKey;
    private string orgId;
    private string model;

    void Awake() {
        LoadSettings();
    }

    private void LoadSettings() {
        // Get the path to the StreamingAssets folder
        string filePath = Path.Combine(Application.streamingAssetsPath, "settings.txt");

        try {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Parse each line
            foreach (string line in lines) {
                // Split the line by '=' and trim whitespace
                string[] parts = line.Split('=');
                if (parts.Length == 2) {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    // Assign values to the appropriate variables
                    switch (key) {
                        case "api_key":
                            apiKey = value;
                            break;
                        case "org_id":
                            orgId = value;
                            break;
                        case "model":
                            model = value;
                            break;
                    }
                }
            }

            // Optional: Log the values to confirm they were read correctly
            Debug.Log($"API Key: {apiKey}");
            Debug.Log($"Org ID: {orgId}");
            Debug.Log($"Model: {model}");
        } catch (System.Exception e) {
            Debug.LogError($"Error reading settings file: {e.Message}");
        }
    }

    // Public getters to access the values from other scripts
    public string GetApiKey() { return apiKey; }
    public string GetOrgId() { return orgId; }
    public string GetModel() { return model; }
}