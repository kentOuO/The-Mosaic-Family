using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    private GameObject currentPuzzleUI; // Reference to the currently active specific Puzzle UI
    private GameObject interactButton; // Reference to the Interact Button UI
    private Animator animator; // Reference to the Animator component
    private UIManager uiManager; // Reference to the UIManager
    private int currentPuzzleIndex; // To keep track of the current puzzle index

    private void Start()
    {
        // Find the UIManager in the scene
        uiManager = FindObjectOfType<UIManager>();

        if (uiManager != null)
        {
            interactButton = uiManager.GetInteractButton(); // Get Interact Button from the manager
        }
        else
        {
            Debug.LogError("UIManager not found in the scene.");
        }

        // Get the player's Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the player object.");
        }
    }

    private void Update()
    {
        // Check if the space key is pressed and if the UI is active
        if (interactButton != null && interactButton.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            // If a puzzle UI is active, exit it
            if (currentPuzzleUI != null)
            {
                ExitPuzzleUI();
                return; // Exit the update function
            }

            // Play interaction animation and delay the puzzle entry
            Debug.Log("Starting interaction, will enter Puzzle UI after delay.");
            if (animator != null)
            {
                animator.SetBool("isInteracting", true); // Set parameter to start interaction
                Invoke("ResetInteracting", 0.1f); // Automatically reset after 0.1 seconds
            }

            // Start coroutine for delayed entry into the Puzzle UI
            StartCoroutine(EnterPuzzleUIDelayed(0.6f)); // 0.5 second delay
        }

        // Check if the Esc key is pressed to exit the Puzzle UI
        if (currentPuzzleUI != null && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPuzzleUI();
        }
    }

    private IEnumerator EnterPuzzleUIDelayed(float delay)
    {
        // Wait for the specified delay (in seconds)
        yield return new WaitForSeconds(delay);

        Debug.Log("Entering Puzzle UI after delay.");

        // Show the Puzzle UI
        currentPuzzleUI = uiManager.GetPuzzleUI(currentPuzzleIndex); // Get the specific Puzzle UI based on index
        if (currentPuzzleUI != null)
        {
            currentPuzzleUI.SetActive(true);
        }

        // Hide the Interact Button
        if (interactButton != null)
        {
            interactButton.SetActive(false);
        }
    }

    private void ExitPuzzleUI()
    {
        Debug.Log("Exiting Puzzle UI");
        currentPuzzleUI.SetActive(false); // Hide the Puzzle UI

        // Show the Interact Button again
        if (interactButton != null)
        {
            interactButton.SetActive(true); // Show Interact Button
        }

        currentPuzzleUI = null; // Reset current puzzle UI

        // Reset interaction animation
        if (animator != null)
        {
            animator.SetBool("isInteracting", false); // Reset interacting parameter
        }
    }

    private void ResetInteracting()
    {
        // Reset isInteracting parameter to false after a brief delay
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

            // Try to get the PuzzleIdentifier component
            PuzzleIdentifier puzzleIdentifier = other.GetComponent<PuzzleIdentifier>();

            // Check if the component exists
            if (puzzleIdentifier != null)
            {
                currentPuzzleIndex = puzzleIdentifier.puzzleIndex; // Set the current puzzle index

                // Show the Interact Button
                if (interactButton != null)
                {
                    interactButton.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("PuzzleIdentifier component is missing on the Puzzle GameObject.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider has the "Puzzle" tag
        if (other.CompareTag("Puzzle"))
        {
            Debug.Log("Exited Puzzle Collider"); // Debug log

            // Hide the Interact Button
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }

            // Hide Puzzle UI if it's the current one
            if (currentPuzzleUI != null)
            {
                currentPuzzleUI.SetActive(false);
                currentPuzzleUI = null; // Reset the current puzzle UI
            }
        }
    }
}
