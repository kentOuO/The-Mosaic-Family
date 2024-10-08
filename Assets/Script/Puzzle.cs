using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public GameObject puzzleUI; // Reference to the specific puzzle UI for this puzzle

    private void Start()
    {
        // Ensure the specific puzzle UI is hidden at the start
        if (puzzleUI != null)
        {
            puzzleUI.SetActive(false);
        }
    }
}
