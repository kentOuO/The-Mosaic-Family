using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private static List<Vector2> usedPositions = new List<Vector2>(); // Store used positions
    private const float minDistance = 50f; // Minimum distance between food items

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
            // Generate a random position within the plate
            RectTransform plateRect = eventData.pointerEnter.GetComponent<RectTransform>();
            Vector2 randomPosition = GetRandomPositionInPlate(plateRect);
            rectTransform.anchoredPosition = randomPosition; // Set to random position
        }
        else
        {
            // Return to original parent and reset position
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    // Generate a random position within the plate bounds
    private Vector2 GetRandomPositionInPlate(RectTransform plateRect)
    {
        Vector2 plateSize = plateRect.sizeDelta;
        Vector2 randomPosition;

        int attempts = 0; // Count the number of attempts to find a valid position

        do
        {
            float randomX = Random.Range(-plateSize.x / 2, plateSize.x / 2);
            float randomY = Random.Range(-plateSize.y / 2, plateSize.y / 2);
            randomPosition = new Vector2(randomX, randomY);

            attempts++;
        } while (!IsValidPosition(randomPosition) && attempts < 10); // Retry if the position is invalid

        usedPositions.Add(randomPosition); // Add the used position to the list
        return randomPosition;
    }

    // Check if the generated position is far enough from other items
    private bool IsValidPosition(Vector2 position)
    {
        foreach (Vector2 usedPos in usedPositions)
        {
            if (Vector2.Distance(usedPos, position) < minDistance) // Too close to another food item
            {
                return false;
            }
        }
        return true;
    }
}
