using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] puzzleUIs; // Array of Puzzle UIs to assign in the inspector
    public GameObject[] puzzleInteractButtons; // Array of Puzzle Interact Buttons to assign in the inspector
    public GameObject doorInteractButton; // Door Interact Button assigned in the inspector
    public GameObject[] furnitureUIs; // An array of furniture UIs
    public GameObject[] furnitureInteractButtons; // An array of furniture interact buttons

    private void Awake()
    {
        // Hide all Puzzle UIs at the start
        foreach (var puzzleUI in puzzleUIs)
        {
            if (puzzleUI != null)
            {
                puzzleUI.SetActive(false);
            }
            else
            {
                Debug.LogError("One of the Puzzle UI GameObjects is not assigned in the UIManager.");
            }
        }

        // Hide all Puzzle Interact Buttons at the start
        foreach (var interactButton in puzzleInteractButtons)
        {
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }
            else
            {
                Debug.LogError("One of the Puzzle Interact Buttons is not assigned in the UIManager.");
            }
        }

        // Ensure the door interact button is hidden at the start
        if (doorInteractButton != null)
        {
            doorInteractButton.SetActive(false);
        }
        else
        {
            Debug.LogError("Door Interact Button GameObject is not assigned in the UIManager.");
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

    public GameObject GetPuzzleInteractButton(int index)
    {
        if (index >= 0 && index < puzzleInteractButtons.Length)
        {
            return puzzleInteractButtons[index];
        }
        return null; // Return null if index is out of range
    }


    public GameObject GetFurnitureUI(int index)
    {
        if (index >= 0 && index < furnitureUIs.Length)
        {
            return furnitureUIs[index]; // Return the UI GameObject for the specified index
        }
        else
        {
            Debug.LogError("Invalid furniture UI index");
            return null;
        }
    }

    // Method to get the specific furniture interact button based on an index
    public GameObject GetFurnitureInteractButton(int index)
    {
        if (index >= 0 && index < furnitureInteractButtons.Length)
        {
            return furnitureInteractButtons[index]; // Return the interact button GameObject for the specified index
        }
        else
        {
            Debug.LogError("Invalid furniture interact button index");
            return null;
        }
    }

    public GameObject GetDoorInteractButton()
    {
        return doorInteractButton;
    }

}
