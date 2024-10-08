using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject puzzleUI; // Reference to the UI GameObject
    private GameObject currentPuzzleUI; // Reference to the specific puzzle UI currently active

    private void Start()
    {
        // Ensure the UI is hidden at the start
        puzzleUI.SetActive(false);
    }

    private void Update()
    {
        // Check for spacebar input if the UI is active
        if (currentPuzzleUI != null && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Toggling Puzzle UI");

            // Toggle the specific puzzle UI on or off
            bool isActive = currentPuzzleUI.activeSelf;
            currentPuzzleUI.SetActive(!isActive); // Set to the opposite of the current state
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
