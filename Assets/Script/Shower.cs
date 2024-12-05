using UnityEngine;

public class Shower : MonoBehaviour
{
    public ParticleSystem rainEffect; // Reference to the Particle System

    private void Start()
    {
        // Hide the Particle System at the start
        if (rainEffect != null)
        {
            rainEffect.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Character A" tag
        if (collision.CompareTag("CharacterA"))
        {
            // Enable and play the Particle System
            if (rainEffect != null)
            {
                rainEffect.gameObject.SetActive(true);
                rainEffect.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the colliding object has the "Character A" tag
        if (collision.CompareTag("CharacterA"))
        {
            // Stop and hide the Particle System
            if (rainEffect != null)
            {
                rainEffect.Stop();
                rainEffect.gameObject.SetActive(false);
            }
        }
    }
}
