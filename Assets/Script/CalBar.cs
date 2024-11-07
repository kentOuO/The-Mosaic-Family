using UnityEngine;
using UnityEngine.UI;

public class CalBar : MonoBehaviour
{
    public Image redBar;          // The red bar that will be reduced (assign it in the Inspector)
    public float maxHealth = 100f; // Maximum health value
    public float reduceAmount = 10f; // Amount of health to reduce per key press

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Initialize with max health
        UpdateHealthBar();         // Initial update to the health bar UI
    }

    public void DeductHealth(float amount) // Change this to public
    {
        // Reduce the current health by the specified amount
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't drop below 0
        UpdateHealthBar(); // Update the UI
    }

    void UpdateHealthBar()
    {
        // Update the red bar fill amount, moving from right to left
        redBar.fillAmount = currentHealth / maxHealth;
    }
}