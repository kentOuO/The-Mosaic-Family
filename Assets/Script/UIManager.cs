using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] puzzleUIs; // Array of Puzzle UIs to assign in the inspector
    public GameObject interactButton; // Assign this in the inspector

    private void Awake()
    {
        foreach (var puzzleUI in puzzleUIs)
        {
            if (puzzleUI != null)
            {
                puzzleUI.SetActive(false); // Ensure each is hidden at the start
            }
            else
            {
                Debug.LogError("One of the Puzzle UI GameObjects is not assigned in the UIManager.");
            }
        }

        if (interactButton != null)
        {
            interactButton.SetActive(false); // Ensure it's hidden at the start
        }
        else
        {
            Debug.LogError("Interact Button GameObject is not assigned in the UIManager.");
        }
    }

    public GameObject GetPuzzleUI(int index)
    {
        if (index >= 0 && index < puzzleUIs.Length)
        {
            return puzzleUIs[index];
        }
        return null; // Return null if index is out of range
    }

    public GameObject GetInteractButton()
    {
        return interactButton;
    }
}
