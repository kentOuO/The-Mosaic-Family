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

    // Crossfade Canvas and Animator
    public GameObject crossfadeCanvas; // Reference to the Crossfade Canvas GameObject
    private Animator crossfadeAnimator; // Reference to the Animator on the Crossfade Canvas

    private void Start()
    {
        // Find the UIManager in the scene
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            interactButton = uiManager.GetDoorInteractButton(); // Get Door Interact Button from the manager
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

        // Ensure the Crossfade Canvas is assigned and retrieve its Animator
        if (crossfadeCanvas != null)
        {
            crossfadeAnimator = crossfadeCanvas.GetComponent<Animator>();
            crossfadeCanvas.SetActive(false); // Deactivate the canvas initially
        }
        else
        {
            Debug.LogError("Crossfade Canvas is not assigned in the Inspector.");
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
            StartCoroutine(CloseDoorAfterDelay(2f));
            StartCoroutine(HandleCrossfadeAndTeleport());
        }
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);

    if (doorAnimator != null)
    {
        doorAnimator.SetBool("isOpen", false); // Close the door
    }
}

    private IEnumerator HandleCrossfadeAndTeleport()
    {
        // Activate the crossfade canvas and play the FadeIn animation
        if (crossfadeCanvas != null && crossfadeAnimator != null)
        {
            crossfadeCanvas.SetActive(true);
            crossfadeAnimator.SetTrigger("FadeIn");
            yield return new WaitForSeconds(1f); // Wait for FadeIn animation to complete

            crossfadeCanvas.SetActive(false); // Deactivate the canvas after FadeIn animation
        }

        // Show the LoadingPage UI
        if (loadingPageUI != null)
        {
            loadingPageUI.SetActive(true);
        }

        yield return new WaitForSeconds(4f); // Wait for loading duration

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

        // Update the camera bounds
        if (cameraConfiner != null && newCameraBounds != null)
        {
            cameraConfiner.m_BoundingShape2D = newCameraBounds;

            if (virtualCamera != null)
            {
                virtualCamera.transform.position = teleportTarget.position;
            }
        }
        else
        {
            Debug.LogError("Cinemachine Confiner or New Camera Bounds not set.");
        }

        // Hide the LoadingPage UI
        if (loadingPageUI != null)
        {
            loadingPageUI.SetActive(false);
        }

        // Reactivate the crossfade canvas and play the FadeOut animation
        if (crossfadeCanvas != null && crossfadeAnimator != null)
        {
            crossfadeCanvas.SetActive(true);
            crossfadeAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f); // Wait for FadeOut animation to complete

            crossfadeCanvas.SetActive(false); // Deactivate the canvas after FadeOut animation
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
