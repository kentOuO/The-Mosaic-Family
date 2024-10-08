using UnityEngine;

public class Plate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DraggableItem"))
        {
            // Handle logic when an item is placed here (e.g., check if it's the correct item)
        }
    }
}
