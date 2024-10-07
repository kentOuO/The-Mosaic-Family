using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 spawnPosition = new Vector3(-6f, -4.7f, 0f); // Set your desired spawn position here
    public float verticalRange = 1f; // Limit for how much the player can move up or down

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
        // Get input for left and right movement
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Get input for up and down movement
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Update animator's isWalking parameter based on input
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0 || Mathf.Abs(verticalInput) > 0);

        // Flip the character's direction if necessary
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Calculate target Y position and clamp it within the vertical range
        float targetY = Mathf.Clamp(rb.position.y + verticalInput * moveSpeed * Time.deltaTime, initialY - verticalRange, initialY + verticalRange);

        // Apply movement (clamped targetY ensures no boundary shaking)
        rb.velocity = new Vector2(moveInput * moveSpeed, (targetY - rb.position.y) / Time.deltaTime);
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
