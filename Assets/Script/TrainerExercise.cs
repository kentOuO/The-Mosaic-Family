using System.Collections;
using UnityEngine;

public class TrainerExercise : MonoBehaviour
{
    private Animator animator;
    private Vector3 fixedPosition;

    private float previousChoice = -1; // Store the last chosen animation
    private float repeatCount = 0; // Count how many times the same animation has repeated

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

            // Perform the chosen animation using triggers
            switch (randomChoice)
            {
                case 1:
                    animator.SetTrigger("isUp");
                    break;
                case 2:
                    animator.SetTrigger("isDown");
                    break;
                case 3:
                    animator.SetTrigger("isLeft");
                    break;
                case 4:
                    animator.SetTrigger("isRight");
                    break;
            }

            previousChoice = randomChoice; // Update the previous choice
            // Wait for the directional animation to complete
            yield return new WaitForSeconds(1.4f); 

            // Play the idle animation for 1 second
            animator.SetTrigger("isIdle");
            yield return new WaitForSeconds(Random.Range(2f, 4f));// Wait 1 second before the next animation
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
}
