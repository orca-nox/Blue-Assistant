using UnityEngine;

public class NeckLookAt : MonoBehaviour {
    [SerializeField] private Transform neckBone;
    [SerializeField] private float maxAngle = 70f; // Maximum neck rotation angle
    [SerializeField] private float turnSpeed = 5f;

    [SerializeField]
    private Transform player;
    private Quaternion initialRotation;

    void Start() {
        //player = Camera.main.transform; // Or reference your player directly
        initialRotation = neckBone.localRotation;
    }

    void Update() {
        // Get direction to player
        Vector3 targetDirection = player.position - neckBone.position;

        // Create a rotation to look at player
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Calculate angle between current forward and target direction
        float angle = Vector3.Angle(neckBone.forward, targetDirection);

        // Only rotate if within max angle
        if (angle <= maxAngle) {
            // Smoothly rotate towards target
            neckBone.rotation = Quaternion.Slerp(
                neckBone.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }
}