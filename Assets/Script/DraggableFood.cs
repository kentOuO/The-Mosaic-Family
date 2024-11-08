using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition; // Store the original position of the food

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

        // 使用 Physics2D.OverlapCircle 檢測是否在 Plate 檢測區域內
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rectTransform.position, 0.5f); // 半徑可調整
        bool plateDetected = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Plate"))
            {
                plateDetected = true;
                if (!isHealthDeducted)
                {
                    // Deduct health from the health bar
                    CalBar calBar = FindObjectOfType<CalBar>();
                    calBar.DeductHealth(healthDeductionValue);
                    isHealthDeducted = true; // Mark as used
                }
                
                // 將物品設置為 Plate 檢測區域的子物件
                transform.SetParent(collider.transform.parent); // 假設 PlateDetectionArea 是 Plate 的子物件
                break;
            }
        }

        if (!plateDetected)
        {
            // 如果未檢測到 Plate，重置位置並還原健康值
            if (isHealthDeducted)
            {
                CalBar calBar = FindObjectOfType<CalBar>();
                calBar.DeductHealth(-healthDeductionValue); // Increase health back when returning
                isHealthDeducted = false;
            }

            // 返回到原始父物件並重置為原始位置
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
