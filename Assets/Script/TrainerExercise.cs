using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrainerExercise : MonoBehaviour
{
    private Animator animator;
    private Vector3 fixedPosition;

    private float previousChoice = -1; // Store the last chosen animation
    private float repeatCount = 0; // Count how many times the same animation has repeated

    [Header("UI Image Settings")]
    public GameObject[] animatedImages; // Array of images to animate
    public Transform startPoint; // Starting point for the images
    public Transform endPoint; // Endpoint for the images
    public float imageMoveSpeed = 2f; // Speed of image movement

    private bool canAnimateImages = false; // Flag to control when images should animate

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

    // This function is triggered when a player with the "CharacterA" tag enters the box collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CharacterA"))
        {
            canAnimateImages = true; // Allow the images to animate when CharacterA enters the collider
        }
    }

    // This function is triggered when the player with "CharacterA" exits the box collider
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CharacterA"))
        {
            canAnimateImages = false; // Stop the images from animating when CharacterA exits the collider
        }
    }

    IEnumerator RandomlyPlayAnimations()
    {
        while (true)
        {
            // Wait until CharacterA enters the collider
            while (!canAnimateImages)
            {
                yield return null;
            }

            int randomChoice;
            do
            {
                randomChoice = Random.Range(1, 5);
            } while (!IsValidChoice(randomChoice));

            // Perform the chosen animation using triggers
            switch (randomChoice)
            {
                case 1:
                    animator.SetTrigger("isUp");
                    StartCoroutine(AnimateImage(0)); // Trigger image animation for "isUp"
                    break;
                case 2:
                    animator.SetTrigger("isDown");
                    StartCoroutine(AnimateImage(1)); // Trigger image animation for "isDown"
                    break;
                case 3:
                    animator.SetTrigger("isLeft");
                    StartCoroutine(AnimateImage(2)); // Trigger image animation for "isLeft"
                    break;
                case 4:
                    animator.SetTrigger("isRight");
                    StartCoroutine(AnimateImage(3)); // Trigger image animation for "isRight"
                    break;
            }

            previousChoice = randomChoice; // Update the previous choice
            // Wait for the directional animation to complete
            yield return new WaitForSeconds(1.4f);

            // Play the idle animation
            animator.SetTrigger("isIdle");
            yield return new WaitForSeconds(Random.Range(2f, 4f)); // Wait before the next animation
        }
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

        return repeatCount <= 3; // Allow only up to 3 consecutive repeats
    }

    IEnumerator AnimateImage(int imageIndex)
    {
        if (imageIndex < 0 || imageIndex >= animatedImages.Length)
            yield break;

        GameObject image = animatedImages[imageIndex];
        RectTransform imageTransform = image.GetComponent<RectTransform>();

        if (!image.activeSelf)
            image.SetActive(true); // Ensure the image is active

        imageTransform.position = startPoint.position; // Set image to start point

        float journey = 0f;
        while (journey < 1f)
        {
            journey += Time.deltaTime * imageMoveSpeed;
            imageTransform.position = Vector3.Lerp(startPoint.position, endPoint.position, journey);
            yield return null;
        }

        image.SetActive(false); // Deactivate the image after reaching the endpoint
    }
}
