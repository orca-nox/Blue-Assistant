using UnityEngine;

public class ApplicationSettings : MonoBehaviour {
    [SerializeField] private bool startInFullscreen = true;
    [SerializeField] private int targetFrameRate = 60;

    // 16:9 aspect ratio resolutions
    private readonly Vector2Int[] supportedResolutions = new Vector2Int[]
    {
        new Vector2Int(1920, 1080), // 1080p
        new Vector2Int(1600, 900),  // 900p
        new Vector2Int(1366, 768),  // HD
        new Vector2Int(1280, 720)   // 720p
    };

    private void Awake() {
        // Set target frame rate
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0; // Disable VSync to ensure frame rate cap works

        // Set fullscreen mode and find best resolution
        SetFullscreenAndResolution();
    }

    private void SetFullscreenAndResolution() {
        // Get the current screen resolution
        Vector2Int currentScreenResolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);

        // Find the highest supported 16:9 resolution that fits the screen
        Vector2Int targetResolution = supportedResolutions[supportedResolutions.Length - 1]; // Default to lowest resolution

        foreach (Vector2Int resolution in supportedResolutions) {
            if (resolution.x <= currentScreenResolution.x && resolution.y <= currentScreenResolution.y) {
                targetResolution = resolution;
                break;
            }
        }

        // Set the resolution and fullscreen mode
        Screen.SetResolution(
            targetResolution.x,
            targetResolution.y,
            startInFullscreen,
            Screen.currentResolution.refreshRate
        );
    }

    // Optional: Add method to toggle fullscreen
    public void ToggleFullscreen() {
        Screen.fullScreen = !Screen.fullScreen;
    }
}