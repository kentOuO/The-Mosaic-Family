using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] puzzleUIs; // Array of Puzzle UIs
    public GameObject[] puzzleInteractButtons; // Array of Puzzle Interact Buttons
    public GameObject[] furnitureUIs; // Array of Furniture UIs
    public GameObject[] furnitureInteractButtons; // Array of Furniture Interact Buttons
    public GameObject doorInteractButton; // Door Interact Button assigned in the inspector

    // Get the specific Puzzle UI by index
    public GameObject GetPuzzleUI(int index)
    {
        if (index >= 0 && index < puzzleUIs.Length)
        {
            return puzzleUIs[index];
        }
        else
        {
            Debug.LogError("Invalid Puzzle UI index");
            return null;
        }
    }

    // Get the specific Puzzle Interact Button by index
    public GameObject GetPuzzleInteractButton(int index)
    {
        if (index >= 0 && index < puzzleInteractButtons.Length)
        {
            return puzzleInteractButtons[index];
        }
        else
        {
            Debug.LogError("Invalid Puzzle Interact Button index");
            return null;
        }
    }

    // Get the specific Furniture UI by index
    public GameObject GetFurnitureUI(int index)
    {
        if (index >= 0 && index < furnitureUIs.Length)
        {
            return furnitureUIs[index];
        }
        else
        {
            Debug.LogError("Invalid Furniture UI index");
            return null;
        }
    }

    // Get the specific Furniture Interact Button by index
    public GameObject GetFurnitureInteractButton(int index)
    {
        if (index >= 0 && index < furnitureInteractButtons.Length)
        {
            return furnitureInteractButtons[index];
        }
        else
        {
            Debug.LogError("Invalid Furniture Interact Button index");
            return null;
        }
    }
    
    public GameObject GetDoorInteractButton()
    {
        return doorInteractButton;
    }
}
