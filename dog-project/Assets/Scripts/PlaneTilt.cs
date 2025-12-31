using UnityEngine;

public class PlaneTilt : MonoBehaviour
{
    [Header("Tilt Settings")]
    public float maxTiltAngle = 20f; // Degrees
    public float tiltSmoothSpeed = 6f; // Higher = snappier

    float currentTiltX;
    float currentTiltZ;

    void Update()
    {
        float horizontal = Input.GetAxis("Vertical"); // A / D
        float vertical = Input.GetAxis("Horizontal"); // W / S

        // Target angles
        float targetTiltZ = horizontal * maxTiltAngle; // Roll
        float targetTiltX = vertical * maxTiltAngle; // Pitch

        // Smooth toward target
        currentTiltX = Mathf.Lerp(currentTiltX, targetTiltX, Time.deltaTime * tiltSmoothSpeed);
        currentTiltZ = Mathf.Lerp(currentTiltZ, targetTiltZ, Time.deltaTime * tiltSmoothSpeed);

        // Apply local rotation
        transform.localRotation = Quaternion.Euler(currentTiltX, 0f, currentTiltZ);
    }
}
