using UnityEngine;

public class CharacterSwitchTrigger : MonoBehaviour
{
    public CharacterSwitchManager characterSwitchManager; // Reference to the CharacterSwitchManager

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger and it's CharacterA
        if (other.CompareTag("CharacterA")) // Ensure CharacterA is tagged as "CharacterA"
        {
            characterSwitchManager.SwitchToCharacterB(); // Call the method to switch to CharacterB
        }
    }
}
