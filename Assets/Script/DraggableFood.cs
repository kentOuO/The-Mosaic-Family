using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    // 定义固定位置
    private static readonly Vector2[] fixedPositions = new Vector2[]
    {
        new Vector2(-10, 21),  // 食物1的位置
        new Vector2(-24, 0),   // 食物2的位置
        new Vector2(13, 45),  // 食物3的位置
        new Vector2(8, 10)    // 食物4的位置
    };

    public enum FoodType
    {
        Type1,
        Type2,
        Type3,
        Type4
    }

    public FoodType foodType; // 用于指示食物类型

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent; // Store the original parent
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Set transparency
        canvasGroup.blocksRaycasts = false; // Disable raycast detection
        transform.SetParent(originalParent.parent); // Move the item out of Plate so it's always on top
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

        // Check if dropped on a plate
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Plate"))
        {
            // Set as child of the plate
            transform.SetParent(eventData.pointerEnter.transform); 
            // 获取固定位置
            Vector2 fixedPosition = GetFixedPositionForFoodType();
            rectTransform.anchoredPosition = fixedPosition; // Set to fixed position
        }
        else
        {
            // Return to original parent and reset position
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    // 获取每种食物类型的固定位置
    private Vector2 GetFixedPositionForFoodType()
    {
        return fixedPositions[(int)foodType]; // 返回对应食物类型的固定位置
    }
}
