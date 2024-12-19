using UnityEngine;

public class ToiletInteraction : MonoBehaviour
{
    private Animator animator;
    private Vector2 swipeStartPos;
    private bool isSwiping;

    [SerializeField]
    private BoxCollider2D detectionCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (detectionCollider == null)
        {
            detectionCollider = GetComponent<BoxCollider2D>();
        }

        if (detectionCollider == null)
        {
            Debug.LogError("No BoxCollider2D attached or assigned!");
        }
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (detectionCollider.OverlapPoint(mousePosition))
            {
                swipeStartPos = mousePosition;
                isSwiping = true;
            }
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping) // Left mouse button released
        {
            Vector2 mouseEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectSwipe(mouseEndPos);
            isSwiping = false;
        }
    }

    private void DetectSwipe(Vector2 endPos)
    {
        float verticalSwipeDistance = endPos.y - swipeStartPos.y;

        if (Mathf.Abs(verticalSwipeDistance) > 0.5f) // Threshold to avoid accidental triggers
        {
            if (verticalSwipeDistance > 0) // Swipe up
            {
                animator.SetBool("isOpen", true);
            }
            else if (verticalSwipeDistance < 0) // Swipe down
            {
                animator.SetBool("isOpen", false);
            }
        }
    }
}
