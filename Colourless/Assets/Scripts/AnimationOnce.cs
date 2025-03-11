using UnityEngine;

public class AnimationOnce : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int frameIndex = 0;
    public int frameRate = 8; // Frames per second

    private float timer = 0f; // Changed to float

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0]; // Set initial sprite
        }
    }

    void Update()
    {
        if (frameIndex >= sprites.Length) // If animation is done, destroy object
        {
            Destroy(gameObject);
            return;
        }

        timer += Time.deltaTime;

        if (timer >= (1f / frameRate)) // Change frame every (1 / frameRate) seconds
        {
            timer = 0f;
            frameIndex++;

            if (frameIndex < sprites.Length)
            {
                spriteRenderer.sprite = sprites[frameIndex];
            }
        }
    }
}
