using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Use specific keys for movement (A and D for left and right)
        movement.x = 0; // Reset movement.x to prevent unintended movement

        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1; // Move left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1; // Move right
        }

        // Update animator's isWalking parameter
        animator.SetBool("isWalking", movement.x != 0);
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}