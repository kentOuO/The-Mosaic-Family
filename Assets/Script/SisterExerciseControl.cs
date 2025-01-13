using System.Collections;
using UnityEngine;

public class SisterExerciseControl : MonoBehaviour
{
    private Animator animator;         // Reference to the Animator component
    private bool isInTrainerZone = false; // Flag to check if the player is in the Trainer zone

    public float normalSpeed = 1f;     // Normal animation speed
    public float maxSpeed = 4f;        // Maximum animation speed
    private Coroutine speedIncreaseCoroutine; // Reference to the speed increase coroutine

    private string lastTrainerAction = ""; // Store last trainer's action for comparison
    private int score = 0; // Track score

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from the player!");
        }
        animator.speed = normalSpeed; // Initialize animation speed to normal
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
            lastTrainerAction = "Up";
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayAnimation("isDown");
            lastTrainerAction = "Down";
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayAnimation("isRight");
            lastTrainerAction = "Right";
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayAnimation("isLeft");
            lastTrainerAction = "Left";
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
        yield return new WaitForSeconds(1.4f / animator.speed);

        // Set isIdle to true to return to the idle animation
        animator.SetBool("isIdle", true);
    }

    // Detect when the player enters the Trainer zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trainer"))
        {
            isInTrainerZone = true;

            // Start increasing animation speed
            if (speedIncreaseCoroutine == null)
            {
                speedIncreaseCoroutine = StartCoroutine(IncreaseAnimationSpeed());
            }
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

            // Stop increasing animation speed and reset it
            if (speedIncreaseCoroutine != null)
            {
                StopCoroutine(speedIncreaseCoroutine);
                speedIncreaseCoroutine = null;
            }
            animator.speed = normalSpeed; // Reset animation speed to normal
        }
    }

    // Coroutine to gradually increase animation speed
    private IEnumerator IncreaseAnimationSpeed()
    {
        float incrementStep = 0.3f;   // Increment step for increasing speed
        float incrementDelay = 5f;   // Delay between each increment

        while (animator.speed < maxSpeed)
        {
            animator.speed += incrementStep;
            animator.speed = Mathf.Clamp(animator.speed, normalSpeed, maxSpeed); // Clamp to max speed
            yield return new WaitForSeconds(incrementDelay); // Wait for a small delay
        }
    }

    // Check if the Sister pressed the correct button when the image is detected
    public void CheckAction(int imageIndex)
    {
        if (lastTrainerAction == "Up" && imageIndex == 0) // Trainer action is "Up" and the image is "Up"
        {
            score += 10; // Add score
            Debug.Log("Correct! Score: " + score);
        }
        else if (lastTrainerAction == "Down" && imageIndex == 1) // Trainer action is "Down" and the image is "Down"
        {
            score += 10;
            Debug.Log("Correct! Score: " + score);
        }
        else if (lastTrainerAction == "Left" && imageIndex == 2) // Trainer action is "Left" and the image is "Left"
        {
            score += 10;
            Debug.Log("Correct! Score: " + score);
        }
        else if (lastTrainerAction == "Right" && imageIndex == 3) // Trainer action is "Right" and the image is "Right"
        {
            score += 10;
            Debug.Log("Correct! Score: " + score);
        }
        else
        {
            Debug.Log("Miss! Score remains: " + score);
        }
    }
}
