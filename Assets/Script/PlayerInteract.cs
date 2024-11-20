using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour
{
    private GameObject currentPuzzleUI; // Reference to the currently active specific Puzzle UI
    private GameObject currentInteractButton; // Reference to the specific Puzzle Interact Button UI
    private GameObject currentFurnitureUI; // Reference to the currently active specific Furniture UI
    private GameObject currentFurnitureButton; // Reference to the specific Furniture Interact Button UI
    private Animator animator; // Reference to the Animator component
    private UIManager uiManager; // Reference to the UIManager
    private int currentPuzzleIndex; // To keep track of the current puzzle index
    private int currentFurnitureIndex; // To keep track of the current furniture index

    private void Start()
    {
        // Find the UIManager in the scene
        uiManager = FindObjectOfType<UIManager>();

        if (uiManager != null)
        {
            // Ensure the button is null initially
            currentInteractButton = null;
            currentFurnitureButton = null;
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
        // Check if the E key is pressed and if the UI is active
        if (currentInteractButton != null && currentInteractButton.activeSelf && Input.GetKeyDown(KeyCode.E))
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

        // Check if the E key is pressed and if the furniture UI is active
        if (currentFurnitureButton != null && currentFurnitureButton.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            // If a furniture UI is active, exit it
            if (currentFurnitureUI != null)
            {
                ExitFurnitureUI();
                return; // Exit the update function
            }

            // Play interaction animation and delay the furniture entry
            Debug.Log("Starting interaction with furniture, will enter Furniture UI after delay.");
            if (animator != null)
            {
                animator.SetBool("isInteracting", true); // Set parameter to start interaction
                Invoke("ResetInteracting", 0.1f); // Automatically reset after 0.1 seconds
            }

            // Start coroutine for delayed entry into the Furniture UI
            StartCoroutine(EnterFurnitureUIDelayed(0.6f)); // 0.5 second delay
        }

        // Check if the Esc key is pressed to exit the Puzzle or Furniture UI
        if (currentPuzzleUI != null && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPuzzleUI();
        }
        if (currentFurnitureUI != null && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitFurnitureUI();
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
        if (currentInteractButton != null)
        {
            currentInteractButton.SetActive(false);
        }
    }

    private IEnumerator EnterFurnitureUIDelayed(float delay)
    {
        // Wait for the specified delay (in seconds)
        yield return new WaitForSeconds(delay);

        Debug.Log("Entering Furniture UI after delay.");

        // Show the Furniture UI
        currentFurnitureUI = uiManager.GetFurnitureUI(currentFurnitureIndex); // Get the specific Furniture UI based on index
        if (currentFurnitureUI != null)
        {
            currentFurnitureUI.SetActive(true);
        }

        // Hide the Interact Button
        if (currentFurnitureButton != null)
        {
            currentFurnitureButton.SetActive(false);
        }
    }

    private void ExitPuzzleUI()
    {
        Debug.Log("Exiting Puzzle UI");
        currentPuzzleUI.SetActive(false); // Hide the Puzzle UI

        // Show the Interact Button again
        if (currentInteractButton != null)
        {
            currentInteractButton.SetActive(true); // Show Interact Button
        }

        currentPuzzleUI = null; // Reset current puzzle UI

        // Reset interaction animation
        if (animator != null)
        {
            animator.SetBool("isInteracting", false); // Reset interacting parameter
        }
    }

    private void ExitFurnitureUI()
    {
        Debug.Log("Exiting Furniture UI");
        currentFurnitureUI.SetActive(false); // Hide the Furniture UI

        // Show the Interact Button again
        if (currentFurnitureButton != null)
        {
            currentFurnitureButton.SetActive(true); // Show Interact Button
        }

        currentFurnitureUI = null; // Reset current furniture UI

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

                // Get the specific interact button for the current puzzle
                currentInteractButton = uiManager.GetPuzzleInteractButton(currentPuzzleIndex);

                // Show the Interact Button
                if (currentInteractButton != null)
                {
                    currentInteractButton.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("PuzzleIdentifier component is missing on the Puzzle GameObject.");
            }
        }

        // Check if the collider has the "Furniture" tag
        if (other.CompareTag("Furniture"))
        {
            Debug.Log("Entered Furniture Collider"); // Debug log

            // Try to get the FurnitureIdentifier component
            FurnitureIdentifier furnitureIdentifier = other.GetComponent<FurnitureIdentifier>();

            // Check if the component exists
            if (furnitureIdentifier != null)
            {
                currentFurnitureIndex = furnitureIdentifier.furnitureIndex; // Set the current furniture index

                // Get the specific interact button for the current furniture
                currentFurnitureButton = uiManager.GetFurnitureInteractButton(currentFurnitureIndex);

                // Show the Interact Button
                if (currentFurnitureButton != null)
                {
                    currentFurnitureButton.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("FurnitureIdentifier component is missing on the Furniture GameObject.");
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
            if (currentInteractButton != null)
            {
                currentInteractButton.SetActive(false);
            }

            // Hide Puzzle UI if it's the current one
            if (currentPuzzleUI != null)
            {
                currentPuzzleUI.SetActive(false);
                currentPuzzleUI = null; // Reset the current puzzle UI
            }
        }

        // Check if the collider has the "Furniture" tag
        if (other.CompareTag("Furniture"))
        {
            Debug.Log("Exited Furniture Collider"); // Debug log

            // Hide the Interact Button
            if (currentFurnitureButton != null)
            {
                currentFurnitureButton.SetActive(false);
            }

            // Hide Furniture UI if it's the current one
            if (currentFurnitureUI != null)
            {
                currentFurnitureUI.SetActive(false);
                currentFurnitureUI = null; // Reset the current furniture UI
            }
        }
    }
}
