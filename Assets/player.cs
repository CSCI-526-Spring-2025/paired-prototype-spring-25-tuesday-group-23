using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    public Ball ball;
    private float movementX;
    private float angle;
    private float strength;
    public float speed = 5f;
    private bool spaceReleased = true;
    private bool playerInAir = false;
    private int min_strength = 15;
    private int max_strength = 30;

    public GameObject arrow;
    private RectTransform arrowTransform;
    public Slider strengthBar;
    private bool playerinair=false;
    private Transform movingBlock; 
    private Vector2 lastBlockPosition;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(-7.5f, -3.5f);
        arrowTransform = arrow.GetComponent<RectTransform>();
        arrow.SetActive(true);
        strengthBar.minValue = min_strength;
        strengthBar.maxValue = max_strength;
        strengthBar.gameObject.SetActive(true);
        ResetShootingParameters();
    }

    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");

        if (!playerInAir) // Only adjust angle and strength if player is on the ground
        {
            // Adjust Angle (W/S)
            if (Input.GetKey(KeyCode.W))
            {
                angle += 60f * Time.deltaTime;
                if (angle > 160f) angle = 160f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                angle -= 60f * Time.deltaTime;
                if (angle < 20f) angle = 20f;
            }

            // Adjust Strength (A/D)
            if (Input.GetKey(KeyCode.A))
            {
                strength -= 10f * Time.deltaTime;
                if (strength < min_strength) strength = min_strength;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                strength += 10f * Time.deltaTime;
                if (strength > max_strength) strength = max_strength;
            }

            // Update UI elements
            arrow.SetActive(true);
            strengthBar.gameObject.SetActive(true);
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle);
            strengthBar.value = strength;
        }

        // Shoot Ball (Space) - Only when player is on ground
        if (Input.GetKeyDown(KeyCode.Space) && spaceReleased && !playerInAir)
        {
            ball.ReleaseBall(strength, angle);
            Debug.Log("Ball Shot! Angle: " + angle + " Strength: " + strength);
            spaceReleased = false;  // Prevent shooting multiple times
            arrow.SetActive(false);
            strengthBar.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!playerInAir)
        {
            rb.velocity = new Vector2(movementX * speed * 5, 0);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop movement when in air
        }

        // Keep UI elements near the player
        arrow.transform.position = new Vector2(rb.position.x + 0.75f, rb.position.y + 0.9f);
        strengthBar.transform.position = new Vector2(rb.position.x + 0.75f, rb.position.y + 1.7f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Existing behavior for resetting the shooting parameters
        if (collision.gameObject.CompareTag("block") || collision.gameObject.CompareTag("topbotwall") || collision.gameObject.CompareTag("spike"))
        {
            playerInAir = false;
            spaceReleased = true;  // Allow shooting again when back on ground
            ResetShootingParameters();

            // If the block is moving, set the player as a child of the block
            MovingBlock movingScript = collision.gameObject.GetComponent<MovingBlock>();
            if (movingScript != null)
            {
                transform.parent = collision.transform;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("block") || collision.gameObject.CompareTag("topbotwall"))
        {
            playerInAir = true;

            // If the block was moving, unparent the player so it no longer follows
            MovingBlock movingScript = collision.gameObject.GetComponent<MovingBlock>();
            if (movingScript != null)
            {
                transform.parent = null;
            }
        }
    }


    public void ResetShootingParameters()
    {
        angle = 90f; // Reset angle after landing
        strength = min_strength; // Reset strength after landing
        arrow.SetActive(true);
        strengthBar.gameObject.SetActive(true);
        spaceReleased = true;
    }
    
}
