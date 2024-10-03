using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 spawnPosition = new Vector3(-6f, -3f, 0f); // Set your desired spawn position here

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set the initial spawn position
        transform.position = spawnPosition;
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
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
