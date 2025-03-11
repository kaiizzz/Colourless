using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed = 2f; // Speed of conveyor belt movement

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(speed, playerRb.velocity.y); // Apply conveyor force
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(0, playerRb.velocity.y); // Stop movement when leaving
            }
        }
    }
}
