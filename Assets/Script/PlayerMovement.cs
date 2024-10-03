using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true; // Track if the character is facing right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input for left and right movement
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Update animator's isWalking parameter based on input
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0);

        // Flip the character's direction if necessary
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Apply movement
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    // Method to flip the character's direction
    void Flip()
    {
        isFacingRight = !isFacingRight; // Toggle the direction
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; // Flip the x-scale of the sprite
        transform.localScale = scaler;
    }
}