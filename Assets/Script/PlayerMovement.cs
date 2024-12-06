using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 3f;           // Normal movement speed
    public float runSpeed = 6f;            // Speed when running
    public Vector3 spawnPosition = new Vector3(-6f, -4.7f, 0f); // Set your desired spawn position here
    public float verticalRange = 0.7f;        // Limit for how much the player can move up or down

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private float initialY; // Initial Y position to limit vertical movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set the initial spawn position
        transform.position = spawnPosition;

        // Store the initial Y position
        initialY = transform.position.y;

        // Remove gravity
        rb.gravityScale = 0;
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Calculate movement speed
        float currentSpeed = Mathf.Sqrt(moveInput * moveInput + verticalInput * verticalInput) * (isRunning ? runSpeed : moveSpeed);

        // Update Animator parameters
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("isRunning", isRunning);

        // Flip the character if necessary
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Clamp vertical movement and apply position changes
        float targetY = Mathf.Clamp(rb.position.y + verticalInput * currentSpeed * Time.deltaTime, initialY - verticalRange, initialY + verticalRange);
        rb.position = new Vector2(rb.position.x + moveInput * currentSpeed * Time.deltaTime, targetY);
    }

    // Method to flip the character's direction
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
