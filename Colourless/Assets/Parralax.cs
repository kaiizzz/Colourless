using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public Transform[] backgrounds; // Assign your background layers
    public float[] parallaxScales; // Speed multiplier for each background layer
    public float smoothing = 1f; // Adjust for smoothness

    public Transform player; // Reference to the player
    public float dashMultiplier = 2f; // Speed increase when dashing

    private Transform cam; // Reference to the camera
    private Vector3 previousCamPos; // Previous camera position
    private float defaultSpeed = 1f; // Base movement speed
    private bool isDashing = false; // Tracks if the player is dashing

    private void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;

        // Auto-set parallax scales if not manually assigned
        if (parallaxScales.Length != backgrounds.Length)
        {
            parallaxScales = new float[backgrounds.Length];
            for (int i = 0; i < backgrounds.Length; i++)
            {
                parallaxScales[i] = (i + 1) * 0.2f; // Adjust layer depth effect
            }
        }
    }

    private void Update()
    {
        float speedMultiplier = isDashing ? dashMultiplier : defaultSpeed; // Speed up when dashing

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i] * speedMultiplier;

            Vector3 newPos = new Vector3(backgrounds[i].position.x + parallax, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, newPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position; // Update previous camera position
    }

    // Call this from PlayerController when dashing
    public void SetDashing(bool state)
    {
        isDashing = state;
    }
}
