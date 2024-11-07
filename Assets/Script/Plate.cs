using UnityEngine;

public class Plate : MonoBehaviour
{
    public CalBar healthBar; // Reference to the CalBar script

    // Method to deduct health
    public void DeductHealth(float amount)
    {
        if (healthBar != null)
        {
            healthBar.DeductHealth(amount); // Call the method to deduct health from the CalBar
        }
    }
}
