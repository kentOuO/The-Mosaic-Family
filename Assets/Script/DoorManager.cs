using UnityEngine;
using Cinemachine;
using System.Collections;

public class DoorManager : MonoBehaviour
{
    private Animator doorAnimator; // Reference to the door's Animator component
    public Transform teleportTarget; // Teleport target for player
    public float teleportDelay = 0.6f; // Delay before teleportation
    public float doorOpenDelay = 0.6f; // Delay for door animation

    private GameObject interactButton; // Reference to the Interact Button UI
    private UIManager uiManager; // Reference to the UIManager

    private bool playerInRange = false; // Flag to check if player is near the door

    // Camera References
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    public CinemachineConfiner2D cameraConfiner; // Reference to the Cinemachine Confiner 2D
    public PolygonCollider2D newCameraBounds; // New camera bounds (polygon collider 2D for the new area)

    // LoadingPage UI Reference
    public GameObject loadingPageUI; // Reference to the LoadingPage UI Canvas

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

        // Ensure the Cinemachine Virtual Camera and Camera Confiner are set
        if (virtualCamera == null)
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (cameraConfiner == null)
            cameraConfiner = FindObjectOfType<CinemachineConfiner2D>();
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
            StartCoroutine(TeleportAndMoveCameraAfterDelay(teleportDelay)); // Teleport player and move camera after delay
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

    private IEnumerator TeleportAndMoveCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Teleport the player
        if (teleportTarget != null)
        {
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

        // Move the camera to the new bounds
        if (cameraConfiner != null && newCameraBounds != null)
        {
            // Update the camera confiner to the new polygon bounds
            cameraConfiner.m_BoundingShape2D = newCameraBounds;

            // Set the virtual camera to the new position (same position as teleport target)
            if (virtualCamera != null)
            {
                virtualCamera.transform.position = teleportTarget.position;
            }
        }
        else
        {
            Debug.LogError("Cinemachine Confiner or New Camera Bounds not set.");
        }

        // Show the LoadingPage UI for 3 seconds
        if (loadingPageUI != null)
        {
            loadingPageUI.SetActive(true); // Show LoadingPage
            yield return new WaitForSeconds(4f); // Wait for 3 seconds
            loadingPageUI.SetActive(false); // Hide LoadingPage
        }
        else
        {
            Debug.LogError("LoadingPage UI is not assigned.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CharacterA")) // If the player enters the door collider
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
        if (other.CompareTag("CharacterA")) // If the player exits the door collider
        {
            playerInRange = false; // Player is no longer near the door
            if (interactButton != null)
            {
                interactButton.SetActive(false); // Hide the interact button UI
            }
        }
    }
}