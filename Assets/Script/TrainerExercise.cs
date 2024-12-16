using System.Collections;
using UnityEngine;

public class TrainerExercise : MonoBehaviour
{
    private Animator animator;
    private Vector3 fixedPosition;

    private int previousChoice = -1; // Store the last chosen animation
    private int repeatCount = 0; // Count how many times the same animation has repeated

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
            ResetBooleans();

            int randomChoice;
            do
            {
                randomChoice = Random.Range(1, 5);
            } while (!IsValidChoice(randomChoice));

            // Perform the chosen animation
            switch (randomChoice)
            {
                case 1:
                    animator.SetBool("isUp", true);
                    break;
                case 2:
                    animator.SetBool("isDown", true);
                    break;
                case 3:
                    animator.SetBool("isLeft", true);
                    break;
                case 4:
                    animator.SetBool("isRight", true);
                    break;
            }

            previousChoice = randomChoice; // Update the previous choice
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

    void ResetBooleans()
    {
        animator.SetBool("isUp", false);
        animator.SetBool("isDown", false);
        animator.SetBool("isLeft", false);
        animator.SetBool("isRight", false);
    }
}
