using UnityEngine;

public class CharacterSwitchManager : MonoBehaviour
{
    public GameObject characterAPrefab; // The first character prefab
    public GameObject characterBPrefab;  // The second character prefab
    private GameObject currentCharacter; // The currently active character

    private void Start()
    {
        // Instantiate the first character at the start
        currentCharacter = Instantiate(characterAPrefab, transform.position, Quaternion.identity);
    }

    // Method to switch to the second character
    public void SwitchToCharacterB()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter); // Destroy the current character
        }

        // Instantiate the new character (CharacterB)
        currentCharacter = Instantiate(characterBPrefab, transform.position, Quaternion.identity);
    }
}
