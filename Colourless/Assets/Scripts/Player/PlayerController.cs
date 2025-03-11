using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
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

    [Header("Physics")]
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private int remainingDashes;
    private float originalGravity;
    private float dashTime;
    private Vector2 dashDirection;
    private int remainingJumps = 1;

    [Header("Checkpoints")]
    public Transform checkPoint;
    

    [Header("Color Energy")]
    private ColorEnergySystem colorEnergy; // Reference to energy system
    public SpawnEnergy[] spawnEnergy;


    [Header("Particles/Effects")]
    public GameObject groundParticles;
    private GameObject groundParticlesInstance;
    private int instances =0;
    public GameObject explosion;
    private GameObject explosionInstance;
    public GameObject destroyobject;
    public GameObject djParticles;
    private GameObject djParticlesInstance;
    public GameObject BlastEnergy;
    public GameObject JumpEffect;
    private GameObject JumpEffectInstance;
    public GameObject DashEffect;
    private GameObject DashEffectInstance;
    public GameObject DashEffectGround;
    private GameObject DashEffectGroundInstance;
    public GameObject LandingEffect;
    private GameObject LandingEffectInstance;

    [Header("Corrosion")]
    private bool inCorrosion = false;
    private bool holdingDownK = false;

    [Header("Sprites and Animation")]
    public Sprite[] sprite;
    public SpriteRenderer spriteRendererMask;
    public Sprite[] maskSprite;

    public GameObject prompt;

    public GameObject Shield;
    private GameObject ShieldInstance;

    public GameObject JumpSoundEffect;
    private GameObject JumpSoundEffectInstance;
    public GameObject DashSoundEffect;
    private GameObject DashSoundEffectInstance;
    public GameObject DeadSoundEffect;
    private GameObject DeadSoundEffectInstance;
    public GameObject ExplodeSoundEffect;
    private GameObject ExplodeSoundEffectInstance;

    public GameObject EnergyCollectionSoundEffect;
    private GameObject EnergyCollectionSoundEffectInstance;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        remainingDashes = maxDashes;
        colorEnergy = GetComponent<ColorEnergySystem>(); // Make sure there's an energy system component

        // Find all spawn energy objects in the scene
        spawnEnergy = FindObjectsOfType<SpawnEnergy>();
    }


    private float energyTimer = 0f;
    private void Update()
    {
        energyTimer += Time.deltaTime;
        
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
            rb.velocity = Vector2.zero;
            StartCoroutine(Respawn());
        }

        if (Input.GetKey(KeyCode.K) && !colorEnergy.outOfEnergy) {
            holdingDownK = true; 
            if (ShieldInstance == null)
            {
                ShieldInstance = Instantiate(Shield, transform.position, Quaternion.identity);
                ShieldInstance.transform.parent = transform;
            }
            if (energyTimer >= 1f)  // Every 1 second
            {
                colorEnergy.UseEnergy(5f);
                energyTimer = 0f; // Reset timer
            }
        } else {
            holdingDownK = false;
            if (ShieldInstance != null)
            {
                Destroy(ShieldInstance);
            }
        }
        
        if (!holdingDownK && inCorrosion) {
            inCorrosion = false;

            if (DeadSoundEffect != null)
            {
                DeadSoundEffectInstance = Instantiate(DeadSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(DeadSoundEffectInstance);
                DeadSoundEffectInstance = Instantiate(DeadSoundEffect, transform.position, Quaternion.identity);
            }

            if (ExplodeSoundEffect != null)
            {
                ExplodeSoundEffectInstance = Instantiate(ExplodeSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(ExplodeSoundEffectInstance);
                ExplodeSoundEffectInstance = Instantiate(ExplodeSoundEffect, transform.position, Quaternion.identity);
            }
            
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
            rb.velocity = Vector2.zero;
            StartCoroutine(Respawn());
        }

        HandleSprites();

        if (canAcivate) {
            prompt.SetActive(true);
            if (Input.GetKeyDown(KeyCode.L)) {
                targetAlter.GetComponent<ActivateAlter>().isOn = true;
                canAcivate = false;
            }
        } else {
            prompt.SetActive(false);
        }
    }

    private float lastDirection = 1f; // 1 = right, -1 = left

    private void HandleSprites()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float moveInput = Input.GetAxis("Horizontal");

        // Running right
        if (moveInput > 0)
        {
            sr.sprite = sprite[1]; // Running sprite
            spriteRendererMask.sprite = maskSprite[1];
            sr.flipX = false;
            spriteRendererMask.flipX = false;
            lastDirection = 1f;
        }
        // Running left
        else if (moveInput < 0)
        {
            sr.sprite = sprite[1]; // Running sprite
            spriteRendererMask.sprite = maskSprite[1];
            sr.flipX = true;
            spriteRendererMask.flipX = true;
            lastDirection = -1f;
        }
        // Stopping - Switch to Idle but keep facing the last direction
        else
        {
            sr.sprite = sprite[0]; // Idle sprite
            spriteRendererMask.sprite = maskSprite[0];
            sr.flipX = (lastDirection == -1f);
            spriteRendererMask.flipX = (lastDirection == -1f);
        }

        // Jumping (Going Up)
        if (!isGrounded && rb.velocity.y > 0)
        {
            sr.sprite = sprite[2]; // Jumping sprite
            spriteRendererMask.sprite = maskSprite[2];
        }
        // Falling (Going Down)
        else if (!isGrounded && rb.velocity.y < 0)
        {
            sr.sprite = sprite[3]; // Falling sprite
            spriteRendererMask.sprite = maskSprite[3];
        }
    }




    private bool wasInAir = false;
    private bool OnBelt = false;

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

        if (OnBelt)
        {
            rb.velocity = new Vector2(3, 0);
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
            if (JumpSoundEffect != null)
            {
                JumpSoundEffectInstance = Instantiate(JumpSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(JumpSoundEffectInstance);
                JumpSoundEffectInstance = Instantiate(JumpSoundEffect, transform.position, Quaternion.identity);
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && remainingDashes > 0 && colorEnergy.HasEnoughEnergy(colorEnergyCost))
        {
            if (DashSoundEffect != null)
            {
                DashSoundEffectInstance = Instantiate(DashSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(DashSoundEffectInstance);
                DashSoundEffectInstance = Instantiate(DashSoundEffect, transform.position, Quaternion.identity);
            }
            
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

    public Sprite ActiveRespawnSprite;
    private bool canAcivate = false;
    private GameObject targetAlter;

    // 2D trigger collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (DeadSoundEffect != null)
            {
                DeadSoundEffectInstance = Instantiate(DeadSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(DeadSoundEffectInstance);
                DeadSoundEffectInstance = Instantiate(DeadSoundEffect, transform.position, Quaternion.identity);
            }

            if (ExplodeSoundEffect != null)
            {
                ExplodeSoundEffectInstance = Instantiate(ExplodeSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(ExplodeSoundEffectInstance);
                ExplodeSoundEffectInstance = Instantiate(ExplodeSoundEffect, transform.position, Quaternion.identity);
            }

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
            
            // disble player movement
            rb.velocity = Vector2.zero;
            

            StartCoroutine(Respawn());
        }

        if (collision.gameObject.tag == "Corrosion") {
            inCorrosion = true;
        }

        if (collision.gameObject.tag == "Checkpoint")
        {
            if (EnergyCollectionSoundEffect != null)
            {
                EnergyCollectionSoundEffectInstance = Instantiate(EnergyCollectionSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(EnergyCollectionSoundEffectInstance);
                EnergyCollectionSoundEffectInstance = Instantiate(EnergyCollectionSoundEffect, transform.position, Quaternion.identity);
            }
            checkPoint = collision.gameObject.transform;
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = ActiveRespawnSprite;

            // REFILL ENERGY
            colorEnergy.RestoreEnergy(100f);
        }

        if (collision.gameObject.tag == "Energy")
        {
            if (EnergyCollectionSoundEffect != null)
            {
                EnergyCollectionSoundEffectInstance = Instantiate(EnergyCollectionSoundEffect, transform.position, Quaternion.identity);
            } else {
                // destry and reinstantiate
                Destroy(EnergyCollectionSoundEffectInstance);
                EnergyCollectionSoundEffectInstance = Instantiate(EnergyCollectionSoundEffect, transform.position, Quaternion.identity);
            }
            colorEnergy.RestoreEnergy(20f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "TriggerNextLevel1")
        {
           // Evoke scene manager singleton to load next scene
            SceneManagerSingleton.Instance.LoadScene("Level2");
        }

        if (collision.gameObject.tag == "NextLevelTrigger2")
        {
            // Evoke scene manager singleton to load next scene
            SceneManagerSingleton.Instance.LoadScene("End1");
        }

        if (collision.gameObject.tag == "Alter")
        {
            if (!collision.gameObject.GetComponent<ActivateAlter>().isOn)
            {
                Debug.Log("Can activate");
                canAcivate = true;
                targetAlter = collision.gameObject;
            }
        }

        if (collision.gameObject.tag == "ConveyerBelt")
        {
           OnBelt = true;
        }
    }



    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Corrosion")
        {
            inCorrosion = false;
        }

        if (collision.gameObject.tag == "Alter")
        {
            if (!collision.gameObject.GetComponent<ActivateAlter>().isOn)
            {
                canAcivate = false;
                targetAlter = null;
            }
            canAcivate = false;
            targetAlter = null;
        }

        if (collision.gameObject.tag == "ConveyerBelt")
        {
            OnBelt = false;
        }

    }

    IEnumerator Respawn()
    {
        colorEnergy.RestoreEnergy(100f);
        // change alpha to 0
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        // disable player movement  
        moveSpeed = 0;

        
        yield return new WaitForSeconds(1);
        transform.position = checkPoint.position;
        // change alpha back to 1 and flash the player
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        // re-enable player movement
        moveSpeed = 7;

    }
}