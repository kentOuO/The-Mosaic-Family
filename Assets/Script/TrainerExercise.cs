using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrainerExercise : MonoBehaviour
{
    private Animator animator;
    private Vector3 fixedPosition;

    private int previousChoice = -1; // Store the last chosen animation
    private int repeatCount = 0; // Count how many times the same animation has repeated

    public GameObject[] images; // Array to hold the 4 images to spawn
    public Transform startPoint; // Starting point for the images
    public Transform endPoint;   // End point for the images

    void Start()
    {
        animator = GetComponent<Animator>();
        fixedPosition = transform.position; // Record the NPC's current position
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
            int randomChoice;
            do
            {
                randomChoice = Random.Range(1, 5);
            } while (!IsValidChoice(randomChoice));

            // Perform the chosen animation and spawn the corresponding image
            switch (randomChoice)
            {
                case 1:
                    animator.SetTrigger("isUp");
                    SpawnAndMoveImage(0); // Image 0 for "isUp"
                    break;
                case 2:
                    animator.SetTrigger("isDown");
                    SpawnAndMoveImage(1); // Image 1 for "isDown"
                    break;
                case 3:
                    animator.SetTrigger("isLeft");
                    SpawnAndMoveImage(2); // Image 2 for "isLeft"
                    break;
                case 4:
                    animator.SetTrigger("isRight");
                    SpawnAndMoveImage(3); // Image 3 for "isRight"
                    break;
            }

            // Wait for the directional animation to complete
            yield return new WaitForSeconds(1.4f);

            // Play the idle animation
            animator.SetTrigger("isIdle");
            yield return new WaitForSeconds(Random.Range(2f, 4f)); // Wait before the next animation
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

    // Validate whether the animation can be played
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
