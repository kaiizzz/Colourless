using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player reference
    public float smoothSpeed = 0.125f; // Adjust for smoothness
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Camera position relative to player

    [Header("Y Axis Restrictions")]
    public float minY = 0f; // Minimum Y level before the camera stops following Y
    public float resumeFollowY = 0f; // Y level at which camera resumes following Y

    [Header("X Axis Restrictions")]
    public float minX = 0f; // Minimum X level before the camera stops following X
    public float maxX = 0f; // Maximum X level before the camera stops following X

    private Vector3 velocity = Vector3.zero;
    private bool isFollowingY = true;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Follow X position always
        float targetX = desiredPosition.x;
        float targetY = isFollowingY ? desiredPosition.y : transform.position.y; // Lock Y if below minY

        // Check if player is below minY
        if (desiredPosition.y < minY)
        {
            isFollowingY = false; // Stop Y movement
        }
        else if (target.position.y >= resumeFollowY)
        {
            isFollowingY = true; // Resume following Y smoothly
        }

        // Clamp X position

        Vector3 finalPosition = new Vector3(targetX, targetY, desiredPosition.z);
        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref velocity, smoothSpeed);
    }
}
