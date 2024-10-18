using UnityEngine;

public class Plate : MonoBehaviour
{
    public CalBar healthBar; // Reference to the CalBar script

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DraggableItem"))
        {
            // Handle logic when an item is placed here (if needed)
        }
    }

    // Method to deduct health
    public void DeductHealth(float amount)
    {
        if (healthBar != null)
        {
            healthBar.DeductHealth(amount); // Call the method to deduct health from the CalBar
        }
    }
}
