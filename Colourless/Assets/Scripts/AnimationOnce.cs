using System.Collections;
using UnityEngine;

public class AnimationOnce : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int frameIndex = 0;
    public int frameRate = 4; // Frames per second

    private int timer = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0]; // Set initial sprite
    }

    void Update()
    {
        timer++;

        if (timer == frameRate)
        {
            timer = 0;
            frameIndex++;
        }

        if (frameIndex < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frameIndex];
        }

        if (frameIndex >= sprites.Length)
        {
            Destroy(gameObject);
        }
        
    }
}
