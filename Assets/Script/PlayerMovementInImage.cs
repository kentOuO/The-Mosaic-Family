using UnityEngine;

public class PlayerMovementInImage : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of vertical movement
    public float minY = -200f; // Minimum vertical position
    public float maxY = 200f; // Maximum vertical position

    private RectTransform rectTransform;
    private Animator animator; // Reference to the Animator component

    private RockSpawner rockSpawner; // Reference to RockSpawner to access gameStarted status

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        //animator.SetFloat("Speed", 0.0f);

        // Find the RockSpawner component in the scene
        rockSpawner = FindObjectOfType<RockSpawner>();
    }

    void Update()
    {
        // Get vertical input
        float moveY = Input.GetAxis("Vertical");

        // Always allow movement before the game starts and during gameplay
        Vector3 movement = new Vector3(0, moveY, 0) * moveSpeed * Time.deltaTime;
        rectTransform.anchoredPosition += new Vector2(0, movement.y);

        // Clamp the position to keep it within the set area
        Vector3 clampedPosition = rectTransform.anchoredPosition;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        rectTransform.anchoredPosition = clampedPosition;

        // Before the game starts, allow vertical input to trigger the run animation
        if (!rockSpawner.IsGameStarted)
        {
            if (Mathf.Abs(moveY) > 0) // If there is vertical input
            {
                animator.SetFloat("Speed", 1.0f);
            }
            else
            {
                animator.SetFloat("Speed", 0.0f);
            }
        }
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", 1.0f); // Set the Speed parameter in Animator
    }

    // New method to stop the player's movement and animation
    public void StopMovement()
    {
        moveSpeed = 0; // Stop moving
        animator.SetFloat("Speed", 0.0f); // Stop animation
    }
}
