using UnityEngine;

public class PhaseLeech : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float drainAmount = 5f; // How much energy it steals per second
    public float detectionRange = 5f; // How close it needs to be to target the player
    public float knockbackForce = 5f;

    private Transform player;
    private ColorEnergySystem playerEnergy;
    private Rigidbody2D rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerEnergy = player.GetComponent<ColorEnergySystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Move toward player if within range
        if (distanceToPlayer < detectionRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerEnergy.HasEnoughEnergy(drainAmount * Time.deltaTime))
            {
                playerEnergy.UseEnergy(drainAmount * Time.deltaTime);
            }
            else
            {
                // Slow the player's movement when out of energy
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.moveSpeed *= 0.9f; // Reduce speed gradually
                }
            }

        }
    }

    public void Repel(Vector2 direction)
    {
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlastEnergy"))
        {
            Destroy(gameObject); // Destroyed by player's attack
        }
    }
}
