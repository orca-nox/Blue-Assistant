using UnityEngine;

public class PlanaController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private PlanaOutputText outputText;
    [SerializeField] private Animator planaAnimator;  // Plana's animator component

    [Header("Location Points")]
    [SerializeField] private Transform deskPoint;
    [SerializeField] private Transform boardPoint;
    [SerializeField] private Transform wallPoint;
    [SerializeField] private Transform debrisPoint;

    // Location enum matching your actual locations
    private enum Location {
        Desk,
        Board,
        Wall,
        Debris
    }

    // Emotion enum (keeping from previous code)
    private enum Emotion {
        Happy,
        Sad,
        Angry,
        Surprised,
        Neutral
    }

    private Location currentLocation;
    private Emotion currentEmotion;

    // Animation trigger names
    private const string DESK_ANIM = "DeskAnim";
    private const string BOARD_ANIM = "BoardAnim";
    private const string WALL_ANIM = "WallAnim";
    private const string DEBRIS_ANIM = "DebrisAnim";

    public void ProcessLLMResponse(string response) {
        string[] words = response.Split(' ');

        if (words.Length < 3) {
            Debug.LogError("LLM response doesn't contain enough information");
            return;
        }

        // Process location and teleport
        if (System.Enum.TryParse(words[0], true, out Location newLocation)) {
            currentLocation = newLocation;
            TeleportToLocation(newLocation);
        } else {
            Debug.LogWarning($"Unknown location: {words[0]}");
        }

        // Process emotion (keeping from previous code)
        if (System.Enum.TryParse(words[1], true, out Emotion newEmotion)) {
            currentEmotion = newEmotion;
        } else {
            Debug.LogWarning($"Unknown emotion: {words[1]}");
        }

        // Queue the remaining text
        string remainingText = string.Join(" ", words[2..]);
        outputText.PlanaQueueText(remainingText);
    }

    private void TeleportToLocation(Location location) {
        Transform targetPoint = null;
        string animationTrigger = "";

        // Determine target position and animation
        switch (location) {
            case Location.Desk:
                targetPoint = deskPoint;
                animationTrigger = DESK_ANIM;
                break;
            case Location.Board:
                targetPoint = boardPoint;
                animationTrigger = BOARD_ANIM;
                break;
            case Location.Wall:
                targetPoint = wallPoint;
                animationTrigger = WALL_ANIM;
                break;
            case Location.Debris:
                targetPoint = debrisPoint;
                animationTrigger = DEBRIS_ANIM;
                break;
        }

        if (targetPoint != null) {
            // Teleport
            transform.position = targetPoint.position;
            transform.rotation = targetPoint.rotation;

            // Play animation
            planaAnimator.SetTrigger(animationTrigger);
        }
    }

    // Optional: Public methods to get current state (keeping from previous code)
    //public Location GetCurrentLocation() => currentLocation;
    //public Emotion GetCurrentEmotion() => currentEmotion;
}