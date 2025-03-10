using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public int maxDashes = 1; // Limit dashes per air time
    public float colorEnergyCost = 10f; // How much color energy a dash consumes

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private int remainingDashes;
    private float originalGravity;
    private float dashTime;
    private Vector2 dashDirection;

    private ColorEnergySystem colorEnergy; // Reference to energy system

    public GameObject groundParticles;
    private GameObject groundParticlesInstance;
    private int instances =0;

    public Transform checkPoint;
    public GameObject explosion;
    private GameObject explosionInstance;
    public GameObject destroyobject;

    private int remainingJumps = 1;

    public GameObject djParticles;
    private GameObject djParticlesInstance;

    public SpawnEnergy[] spawnEnergy;

    public GameObject BlastEnergy;

    public GameObject JumpEffect;
    private GameObject JumpEffectInstance;

    public GameObject DashEffect;
    private GameObject DashEffectInstance;

    public GameObject DashEffectGround;
    private GameObject DashEffectGroundInstance;

    public GameObject LandingEffect;
    private GameObject LandingEffectInstance;

    private bool inCorrosion = false;
    private bool holdingDownK = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        remainingDashes = maxDashes;
        colorEnergy = GetComponent<ColorEnergySystem>(); // Make sure there's an energy system component

        // Find all spawn energy objects in the scene
        spawnEnergy = FindObjectsOfType<SpawnEnergy>();
    }

    private void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
        HandleDash();
        //CheckWall();
        HandleDoubleJump();

        if (Input.GetKeyDown(KeyCode.L))
        {
            Instantiate(BlastEnergy, transform.position, Quaternion.identity);
            colorEnergy.UseEnergy(20f);

            // if anything in a radius of 5 is tagged UpDown set active to true
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "UpDown")
                {
                    collider.gameObject.GetComponent<UpDownPlatform>().active = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(Respawn());
        }

        if (Input.GetKey(KeyCode.K) && !colorEnergy.outOfEnergy) {
            holdingDownK = true; 
            colorEnergy.UseEnergy(0.1f);
        } else {
            holdingDownK = false;
        }
        
        if (!holdingDownK && inCorrosion) {
            inCorrosion = false;
            
            if (explosion != null)
            {
                explosionInstance = Instantiate(explosion, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
                Instantiate(destroyobject, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);

            }

            for (int i = 0; i < spawnEnergy.Length; i++)
            {
                spawnEnergy[i].Notify();
            }

            // respawn at checkpoint after 1 second
            StartCoroutine(Respawn());
        }

    }

    private bool wasInAir = false;

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            remainingJumps = 1; // Reset jumps when touching the ground
            remainingDashes = maxDashes; // Reset dashes when touching the ground

            // Spawn ground particles if moving
            if (rb.velocity.x >= 0.1f || rb.velocity.x <= -0.1f) 
            {
                if (colorEnergy.GetEnergyPercentage() > 0.1f) 
                {
                    groundParticlesInstance = Instantiate(groundParticles, transform.position, Quaternion.identity);
                    
                    if (instances < 10)
                    {
                        instances++;
                    }
                    else
                    {
                        Destroy(groundParticlesInstance);
                        instances--;
                    }
                }
            }

            // Stop and destroy all double jump particle instances when landing
            if (!isDashing && djParticlesInstance != null)
            {
                GameObject[] allDjParticles = GameObject.FindGameObjectsWithTag("DoubleJumpParticles");
                // set particle array to same length as allDjParticles
                ParticleSystem[] allParticles = new ParticleSystem[allDjParticles.Length];
                // loop through allDjParticles and set allParticles to the particle system of each object
                for (int i = 0; i < allDjParticles.Length; i++)
                {
                    allParticles[i] = allDjParticles[i].GetComponent<ParticleSystem>();
                }
                
                foreach (ParticleSystem ps in allParticles)
                {
                    ps.Stop(); // Stops new particles from spawning
                }
            }

            // ✅ Only spawn the landing effect when landing (not every frame on ground)
            if (wasInAir)  
            {
                if (LandingEffect != null)
                {
                    Instantiate(LandingEffect, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
                }
            }

            wasInAir = false;  // Player is now grounded
        }
        else
        {
            wasInAir = true;  // Player is airborne
        }
    }



    private void HandleMovement()
    {
        if (isDashing) return; // Disable movement during dash

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
                
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && remainingDashes > 0 && colorEnergy.HasEnoughEnergy(colorEnergyCost))
        {
            
            if (djParticles != null)
            {
                djParticlesInstance = Instantiate(djParticles, transform.position, Quaternion.identity);
                djParticlesInstance.transform.parent = transform;
                djParticlesInstance.tag = "DoubleJumpParticles"; // Tag it for cleanup later
            }
            StartDash();
        }

        if (isDashing)
        {
            
            dashTime -= Time.deltaTime;
            rb.velocity = dashDirection * dashSpeed;

            if (dashTime <= 0)
            {
                EndDash();
            }
        }
    }

    private void HandleDoubleJump()
    {
        if (Input.GetButtonDown("Jump") && remainingJumps >= 0 && colorEnergy.HasEnoughEnergy(colorEnergyCost) && !isGrounded)
        {
            JumpEffectInstance = Instantiate(JumpEffect, transform.position, Quaternion.identity);

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            // Spawn a new set of double jump particles each time
            if (djParticles != null)
            {
                djParticlesInstance = Instantiate(djParticles, transform.position, Quaternion.identity);
                djParticlesInstance.transform.parent = transform;
                djParticlesInstance.tag = "DoubleJumpParticles"; // Tag it for cleanup later
            }

            remainingJumps--;
            colorEnergy.UseEnergy(colorEnergyCost);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        remainingDashes--;

        // Get dash direction
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput == 0) horizontalInput = transform.localScale.x; // Dash in facing direction if no input
        dashDirection = new Vector2(horizontalInput, 0).normalized;

        // ✅ Only spawn the dash effect when dashing (not every frame) and in the direction of the dash
        if (isGrounded) {
            if (DashEffectGround != null)
            {
                DashEffectGroundInstance = Instantiate(DashEffectGround, transform.position, Quaternion.identity);
                if (dashDirection.x < 0)
                {
                    DashEffectGroundInstance.transform.localScale = new Vector3(-1, 1, 1);
                }

            }
        }
        else
        {
            if (DashEffect != null)
            {
                DashEffectInstance = Instantiate(DashEffect, transform.position, Quaternion.identity);
                if (dashDirection.x < 0)
                {
                    DashEffectInstance.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

        // Reduce color energy
        colorEnergy.UseEnergy(colorEnergyCost);

        rb.gravityScale = 0; // Disable gravity while dashing
    }

    private void EndDash()
    {
        isDashing = false;
        rb.gravityScale = originalGravity; // Restore gravity
    }

    // 2D trigger collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            //transform.position = checkPoint.position;
            if (explosion != null)
            {
                explosionInstance = Instantiate(explosion, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
                Instantiate(destroyobject, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);

            }

            for (int i = 0; i < spawnEnergy.Length; i++)
            {
                spawnEnergy[i].Notify();
            }

            // respawn at checkpoint after 1 second
            StartCoroutine(Respawn());
        }

        if (collision.gameObject.tag == "Corrosion") {
            inCorrosion = true;
        }

        if (collision.gameObject.tag == "Checkpoint")
        {
            checkPoint = collision.gameObject.transform;
        }

        if (collision.gameObject.tag == "Energy")
        {
            colorEnergy.RestoreEnergy(20f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "TriggerNextLevel1")
        {
           // Evoke scene manager singleton to load next scene
            SceneManagerSingleton.Instance.LoadScene("Level2");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Corrosion")
        {
            inCorrosion = false;
        }
    }

    IEnumerator Respawn()
    {
        // change alpha to 0
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        // disable player movement  
        moveSpeed = 0;

        
        yield return new WaitForSeconds(1);
        transform.position = checkPoint.position;
        // change alpha back to 1 and flash the player
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        // re-enable player movement
        moveSpeed = 7;

    }
}