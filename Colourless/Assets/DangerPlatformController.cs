using System.Collections;
using UnityEngine;

public class DangerPlatformController : MonoBehaviour
{
    public Sprite[] sprites; // [0] = normal, [1] = warning, [2] = disabled
    private bool detected = false;
    private float startTime;
    public float disableDelay = 2f; // Time before platform disappears
    public float respawnDelay = 5f; // Time before platform respawns
    private bool isDisabled = false;
    
    private BoxCollider2D platformCollider;
    private PlatformEffector2D effector;
    
    private void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if (detected && !isDisabled)
        {
            if (Time.time - startTime >= disableDelay)
            {
                StartCoroutine(DisablePlatform());
            }
        }
    }

    private IEnumerator DisablePlatform()
    {
        isDisabled = true;
        GetComponent<SpriteRenderer>().sprite = sprites[2]; // Set to disabled sprite
        platformCollider.enabled = false; // Disable platform collision

        yield return new WaitForSeconds(respawnDelay);

        // Respawn platform
        GetComponent<SpriteRenderer>().sprite = sprites[0]; // Reset to normal sprite
        platformCollider.enabled = true; // Enable collision
        isDisabled = false;
        detected = false; // Reset detection
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDisabled)
        {
            detected = true;
            startTime = Time.time; // Start countdown
            GetComponent<SpriteRenderer>().sprite = sprites[1]; // Warning sprite
        }
    }
}
