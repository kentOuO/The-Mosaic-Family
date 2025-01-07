using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerInteraction : MonoBehaviour
{
    private Animator animator;         // Reference to the Animator component
    private bool isInTrainerZone = false; // Flag to check if the player is in the Trainer zone

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from the player!");
        }
    }

    void Update()
    {
        if (isInTrainerZone)
        {
            HandleTrainerInput();
        }
    }

    // Handle input and set animation parameters
    private void HandleTrainerInput()
    {
        // Check for specific key presses and set the corresponding triggers
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayAnimation("isUp");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayAnimation("isDown");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayAnimation("isRight");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayAnimation("isLeft");
        }
    }

    // Play the specified animation and return to idle state
    private void PlayAnimation(string animationTrigger)
    {
        // Set the specified trigger
        animator.SetTrigger(animationTrigger);

        // Set isIdle to false while the animation is playing
        animator.SetBool("isIdle", false);

        // Return to idle after a short delay (matches animation duration)
        StartCoroutine(ReturnToIdle());
    }

    // Coroutine to return to the idle state
    private IEnumerator ReturnToIdle()
    {
        // Wait for the animation to finish (adjust the duration to match your animation length)
        yield return new WaitForSeconds(1.4f);

        // Set isIdle to true to return to the idle animation
        animator.SetBool("isIdle", true);
    }

    // Detect when the player enters the Trainer zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trainer"))
        {
            isInTrainerZone = true;
        }
    }

    // Detect when the player exits the Trainer zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Trainer"))
        {
            isInTrainerZone = false;

            // Immediately set isIdle to true when leaving the Trainer zone
            animator.SetBool("isIdle", true);
        }
    }
}
