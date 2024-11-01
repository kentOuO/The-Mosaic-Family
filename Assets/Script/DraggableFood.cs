using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition; // Store the original position of the food

    // Add a health deduction value for this food item
    public float healthDeductionValue = 5f; // Set the deduction value for each food type
    private bool isHealthDeducted = false;   // Track if health has been deducted

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent; // Store the original parent
        originalPosition = rectTransform.anchoredPosition; // Store the original position
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Set transparency
        canvasGroup.blocksRaycasts = false; // Disable raycast detection
        transform.SetParent(originalParent.parent); // Move the item out of its original parent so it's always on top
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update position to match the mouse
        Vector2 globalMousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent.GetComponent<RectTransform>(), 
            eventData.position, 
            eventData.pressEventCamera, 
            out globalMousePos))
        {
            rectTransform.anchoredPosition = globalMousePos; // Move item with the mouse
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Restore opacity
        canvasGroup.blocksRaycasts = true; // Re-enable raycast detection

        // If dropped on a plate
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Plate"))
        {
            if (!isHealthDeducted) // Check if health has already been deducted
            {
                // Deduct health from the health bar
                CalBar calBar = FindObjectOfType<CalBar>();
                calBar.DeductHealth(healthDeductionValue);
                isHealthDeducted = true; // Mark as used
            }

            // Set as child of the plate
            transform.SetParent(eventData.pointerEnter.transform);
        }
        else
        {
            // If dragged back to the original parent
            if (isHealthDeducted) // Only restore health if it was deducted
            {
                CalBar calBar = FindObjectOfType<CalBar>();
                calBar.DeductHealth(-healthDeductionValue); // Increase health back when returning
                isHealthDeducted = false; // Reset the deduction flag
            }

            // Return to original parent and reset to the original position if not dropped on a new plate
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition; // Reset to original position
        }
    }
}
