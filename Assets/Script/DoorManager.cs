using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour
{
    private Animator doorAnimator; // Reference to the door's Animator component
    public Transform teleportTarget; // Teleport target for player
    public float teleportDelay = 1.0f; // Delay before teleportation
    public float doorOpenDelay = 1.0f; // Delay for door animation

    private GameObject interactButton; // Reference to the Interact Button UI
    private UIManager uiManager; // Reference to the UIManager

    private bool playerInRange = false; // Flag to check if player is near the door

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

        // Get the door animator component
        doorAnimator = GetComponent<Animator>();
        if (doorAnimator == null)
        {
            Debug.LogError("Animator component is missing on the Door GameObject.");
        }
    }

    private void Update()
    {
        // If player is within range and presses "E", open the door
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoorAndTeleport();
        }
    }

    public void OpenDoorAndTeleport()
    {
        if (doorAnimator != null)
        {
            // Open the door
            doorAnimator.SetBool("isOpen", true);
            StartCoroutine(ResetDoorAfterDelay(doorOpenDelay)); // Reset door animation after delay
            StartCoroutine(TeleportAfterDelay(teleportDelay)); // Teleport player after delay
        }
    }

    private IEnumerator ResetDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("isOpen", false); // Reset the isOpen parameter after the door animation finishes
        }
    }

    private IEnumerator TeleportAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (teleportTarget != null)
        {
            // Teleport the player to the new location
            PlayerInteract playerInteract = FindObjectOfType<PlayerInteract>();
            if (playerInteract != null)
            {
                playerInteract.transform.position = teleportTarget.position;
            }
            else
            {
                Debug.LogError("PlayerInteract component not found.");
            }
        }
        else
        {
            Debug.LogError("Teleport target is not set.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // If the player enters the door collider
        {
            playerInRange = true; // Player is near the door
            if (interactButton != null)
            {
                interactButton.SetActive(true); // Show the interact button UI
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // If the player exits the door collider
        {
            playerInRange = false; // Player is no longer near the door
            if (interactButton != null)
            {
                interactButton.SetActive(false); // Hide the interact button UI
            }
        }
    }
}
