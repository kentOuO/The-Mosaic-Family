using UnityEngine;

public class CharacterSwitchManager : MonoBehaviour
{
    public GameObject characterAPrefab; // The first character prefab
    public GameObject characterBPrefab;  // The second character prefab
    private GameObject currentCharacter; // The currently active character
    private GameObject puzzleUI; // Reference to the UI GameObject

    private void Start()
    {
        // Find the puzzle UI GameObject by tag and set it active
        puzzleUI = GameObject.FindGameObjectWithTag("InteractButton");
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(true); // Activate the UI at the start
        }
        else
        {
            Debug.LogError("Puzzle UI GameObject not found at the start. Make sure it has the 'InteractButton' tag.");
        }

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

        // Find the puzzle UI GameObject again and activate it
        puzzleUI = GameObject.FindGameObjectWithTag("InteractButton");
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(true); // Activate the UI when switching
            Debug.Log("InteractButton activated when switching to Character B.");
        }
        else
        {
            Debug.LogError("Puzzle UI GameObject not found when switching to Character B. Make sure it has the 'InteractButton' tag.");
        }

        // Optionally, initialize the new character's interact script
        PlayerInteract newInteract = currentCharacter.GetComponent<PlayerInteract>();
        if (newInteract != null)
        {
            newInteract.Initialize(); // Reset the UI if necessary
        }
    }
}
