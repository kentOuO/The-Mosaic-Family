using UnityEngine;
using UnityEngine.EventSystems;

public class Toilet : MonoBehaviour, IPointerClickHandler
{
    public GameObject targetToHide; // Specify the GameObject to hide
    public Animator animator; // Animator to control the animation
    private bool isClicked = false;

    void Start()
    {
        // If no target is set, default to hiding the current GameObject
        if (targetToHide == null)
        {
            targetToHide = gameObject;
        }

        // Automatically find the Animator if not set
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError("No Animator component found. Please assign an Animator.");
        }
    }

    void OnEnable()
    {
        // Reset the click state when the GameObject is reactivated
        isClicked = false;

        // Reset the animator state
        if (animator != null)
        {
            animator.SetBool("isClicked", false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClicked && animator != null)
        {
            isClicked = true;

            // Set the "isClicked" bool to true
            animator.SetBool("isClicked", true);

            // Start a coroutine to reset the "isClicked" bool after 1 second
            StartCoroutine(ResetAnimation(1f));

            // Start the coroutine to hide the target GameObject
            StartCoroutine(HideAfterDelay(2f));
        }
    }

    private System.Collections.IEnumerator ResetAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Set the "isClicked" bool back to false
        if (animator != null)
        {
            animator.SetBool("isClicked", false);
        }
    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hide the specified GameObject
        if (targetToHide != null)
        {
            targetToHide.SetActive(false);
        }
    }
}
