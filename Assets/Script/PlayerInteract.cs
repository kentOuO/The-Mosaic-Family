using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    public GameObject puzzleUI; // Reference to the UI GameObject
    private GameObject currentPuzzleUI; // Reference to the specific puzzle UI currently active

    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        // Ensure the UI is hidden at the start
        puzzleUI.SetActive(false);

        // Get the Animator component from the player
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the player object.");
        }
    }

    private void Update()
    {
        // Check for spacebar input if the UI is active
        if (currentPuzzleUI != null && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Toggling Puzzle UI");

            // Play the interaction animation
            if (animator != null)
            {
                animator.SetBool("isInteracting", true); // Set the parameter to start interacting
                Invoke("ResetInteracting", 0.1f); // Automatically reset after 0.1 second
            }

            // Start the coroutine to display the puzzle UI with a delay
            StartCoroutine(DisplayPuzzleUIWithDelay());
        }
    }

    private IEnumerator DisplayPuzzleUIWithDelay()
    {
        yield return new WaitForSeconds(0.7f); // Wait for 0.5 seconds

        // Toggle the specific puzzle UI on or off
        if (currentPuzzleUI != null)
        {
            bool isActive = currentPuzzleUI.activeSelf;
            currentPuzzleUI.SetActive(!isActive); // Set to the opposite of the current state
        }
    }

    private void ResetInteracting()
    {
        // Reset the isInteracting parameter to false after a brief delay
        if (animator != null)
        {
            animator.SetBool("isInteracting", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider has the "Puzzle" tag
        if (other.CompareTag("Puzzle"))
        {
            Debug.Log("Entered Puzzle Collider"); // Debug log

            // Show the generic puzzle UI
            puzzleUI.SetActive(true);

            // Get the specific puzzle UI from the Puzzle component
            Puzzle puzzleComponent = other.GetComponent<Puzzle>();
            if (puzzleComponent != null)
            {
                currentPuzzleUI = puzzleComponent.puzzleUI; // Get the specific puzzle UI
            }
            else
            {
                Debug.LogError("No Puzzle component found on the collider object: " + other.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider has the "Puzzle" tag
        if (other.CompareTag("Puzzle"))
        {
            Debug.Log("Exited Puzzle Collider"); // Debug log

            // Hide the generic puzzle UI
            puzzleUI.SetActive(false);

            // Hide the specific puzzle UI if it was displayed
            if (currentPuzzleUI != null)
            {
                currentPuzzleUI.SetActive(false); // Hide the specific puzzle UI when exiting
            }
            currentPuzzleUI = null; // Clear the reference
        }
    }
}
