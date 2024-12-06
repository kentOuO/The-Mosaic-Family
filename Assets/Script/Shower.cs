using UnityEngine;

public class Shower : MonoBehaviour
{
    public ParticleSystem rainEffect1; // Reference to the first Particle System
    public ParticleSystem rainEffect2; // Reference to the second Particle System

    private void Start()
    {
        // Hide both Particle Systems at the start
        if (rainEffect1 != null)
        {
            rainEffect1.gameObject.SetActive(false);
        }
        if (rainEffect2 != null)
        {
            rainEffect2.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "CharacterA" tag
        if (collision.CompareTag("CharacterA"))
        {
            // Enable and play the first Particle System
            if (rainEffect1 != null)
            {
                rainEffect1.gameObject.SetActive(true);
                rainEffect1.Play();
            }

            // Enable and play the second Particle System
            if (rainEffect2 != null)
            {
                rainEffect2.gameObject.SetActive(true);
                rainEffect2.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the colliding object has the "CharacterA" tag
        if (collision.CompareTag("CharacterA"))
        {
            // Stop and hide the first Particle System
            if (rainEffect1 != null)
            {
                rainEffect1.Stop();
                rainEffect1.gameObject.SetActive(false);
            }

            // Stop and hide the second Particle System
            if (rainEffect2 != null)
            {
                rainEffect2.Stop();
                rainEffect2.gameObject.SetActive(false);
            }
        }
    }
}
