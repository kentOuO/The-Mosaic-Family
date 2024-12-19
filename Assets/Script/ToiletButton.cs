using UnityEngine;

public class PressInteraction : MonoBehaviour
{
    private Animator animator;

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
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (detectionCollider.OverlapPoint(mousePosition))
            {
                animator.SetBool("isPress", true);
                StartCoroutine(ResetIsPress());
            }
        }
    }

    private System.Collections.IEnumerator ResetIsPress()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isPress", false);
    }
}
