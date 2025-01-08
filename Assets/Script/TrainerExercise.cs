using System.Collections;
using UnityEngine;

public class TrainerExercise : MonoBehaviour
{
    private Animator animator;
    private Vector3 fixedPosition;

    public GameObject[] images; // Array to hold the 4 images to spawn
    public Transform startPoint; // Starting point for the images
    public Transform endPoint;   // End point for the images

    private bool canSpawn = false; // Flag to control image spawning
    private bool isPlayerInside = false; // Prevent multiple triggers

    private int previousChoice = -1; // To track the previous animation choice
    private int repeatCount = 0;     // To count consecutive repeats

    public float normalSpeed = 1f;  // Normal animation speed
    public float maxSpeed = 4f;     // Maximum animation speed
    public float idleMinTime = 0f;  // Minimum time between idle animations
    public float idleMaxTime = 4f;  // Maximum time between idle animations

    private Coroutine speedIncreaseCoroutine; // Coroutine reference

    void Start()
    {
        animator = GetComponent<Animator>();
        fixedPosition = transform.position; // Record the NPC's current position
        animator.speed = normalSpeed;       // Initialize animation speed to normal
        StartCoroutine(RandomlyPlayAnimations());
    }

    void Update()
    {
        // Ensure the NPC stays fixed at the original position
        transform.position = fixedPosition;
    }

    IEnumerator RandomlyPlayAnimations()
    {
        while (true)
        {
            int randomChoice = Random.Range(1, 5); // Randomly select an animation (1-4)

            // Check if the chosen animation is valid based on the repeat count
            if (IsValidChoice(randomChoice))
            {
                // Perform the chosen animation and spawn the corresponding image only if canSpawn is true
                switch (randomChoice)
                {
                    case 1:
                        animator.SetTrigger("isUp");
                        if (canSpawn) SpawnAndMoveImage(0); // Image 0 for "isUp"
                        break;
                    case 2:
                        animator.SetTrigger("isDown");
                        if (canSpawn) SpawnAndMoveImage(1); // Image 1 for "isDown"
                        break;
                    case 3:
                        animator.SetTrigger("isLeft");
                        if (canSpawn) SpawnAndMoveImage(2); // Image 2 for "isLeft"
                        break;
                    case 4:
                        animator.SetTrigger("isRight");
                        if (canSpawn) SpawnAndMoveImage(3); // Image 3 for "isRight"
                        break;
                }

                // Wait for the directional animation to complete
                yield return new WaitForSeconds(1.4f / animator.speed);

                // Play the idle animation
                animator.SetTrigger("isIdle");

                // Wait for a random interval before the next animation (minimum idleMinTime)
                float idleWait = Random.Range(idleMinTime, idleMaxTime);
                yield return new WaitForSeconds(idleWait / animator.speed);
            }
            else
            {
                // If the animation is not valid, try again without repeating the previous choice
                yield return null;
            }
        }
    }

    // Spawn and move the image based on animation choice
    void SpawnAndMoveImage(int imageIndex)
    {
        if (imageIndex < images.Length)
        {
            // Instantiate the corresponding image at the start point
            GameObject spawnedImage = Instantiate(images[imageIndex], startPoint.position, Quaternion.identity);
            spawnedImage.transform.SetParent(GameObject.Find("Canvas").transform); // Parent it to the canvas

            // Move the image to the end point over time
            StartCoroutine(MoveImage(spawnedImage, startPoint.position, endPoint.position));
        }
    }

    // Coroutine to move the image from start point to end point and destroy it when it reaches the end point
    IEnumerator MoveImage(GameObject image, Vector3 start, Vector3 end)
    {
        float timeToMove = 2f; // Time for the image to move
        float elapsedTime = 0f;

        while (elapsedTime < timeToMove)
        {
            image.transform.position = Vector3.Lerp(start, end, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.transform.position = end; // Ensure it reaches the end point

        // Destroy the image once it reaches the end point
        Destroy(image);
    }

    // Detect if the player with "CharacterA" tag enters the trigger area
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CharacterA") && !isPlayerInside)
        {
            isPlayerInside = true; // Mark player as inside the trigger
            canSpawn = true;       // Allow image spawning when the player enters the area

            // Start increasing the animation speed gradually
            if (speedIncreaseCoroutine == null)
            {
                speedIncreaseCoroutine = StartCoroutine(IncreaseAnimationSpeed());
            }

            Debug.Log("Player entered the area, animation speed is gradually increasing.");
        }
    }

    // Detect if the player with "CharacterA" tag exits the trigger area
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CharacterA") && isPlayerInside)
        {
            isPlayerInside = false; // Mark player as outside the trigger
            canSpawn = false;       // Stop image spawning when the player exits the area

            // Stop increasing the animation speed and reset it
            if (speedIncreaseCoroutine != null)
            {
                StopCoroutine(speedIncreaseCoroutine);
                speedIncreaseCoroutine = null;
            }
            animator.speed = normalSpeed; // Reset animation speed back to normal

            Debug.Log("Player exited the area, animation speed reset.");
        }
    }

    // Coroutine to gradually increase animation speed
    IEnumerator IncreaseAnimationSpeed()
    {
        float incrementStep = 0.1f;  // Small increments to increase speed
        float incrementDelay = 2f; // Delay between each increment

        while (animator.speed < maxSpeed)
        {
            animator.speed += incrementStep; // Gradually increase speed
            animator.speed = Mathf.Clamp(animator.speed, normalSpeed, maxSpeed); // Clamp to max speed
            yield return new WaitForSeconds(incrementDelay); // Wait for a small delay
        }
    }

    // Method to check if the animation choice is valid (not repeating more than 3 times)
    bool IsValidChoice(int choice)
    {
        if (choice == previousChoice)
        {
            repeatCount++;
        }
        else
        {
            repeatCount = 1; // Reset the count if it's a different animation
        }

        previousChoice = choice; // Update the previous choice
        return repeatCount <= 3; // Allow only up to 3 consecutive repeats
    }
}
