using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间，以便控制按钮

public class UIManager : MonoBehaviour
{
    public GameObject[] puzzleUIs; // Array of Puzzle UIs
    public GameObject[] puzzleInteractButtons; // Array of Puzzle Interact Buttons
    public GameObject[] furnitureUIs; // Array of Furniture UIs
    public GameObject[] furnitureInteractButtons; // Array of Furniture Interact Buttons
    public GameObject doorInteractButton; // Door Interact Button assigned in the inspector

    // 控制每个家具 UI 页面的索引
    private int currentFurniturePage = 0;

    // 获取特定的 Puzzle UI
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

    // 获取特定的 Puzzle Interact Button
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

    // 获取特定的 Furniture UI
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

    // 获取特定的 Furniture Interact Button
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

    // 获取门的交互按钮
    public GameObject GetDoorInteractButton()
    {
        return doorInteractButton;
    }

    // 显示当前页面的家具 UI
    public void ShowFurnitureUI(int index)
    {
        if (index >= 0 && index < furnitureUIs.Length)
        {
            furnitureUIs[index].SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid Furniture UI index");
        }
    }

    // 隐藏当前页面的家具 UI
    public void HideFurnitureUI(int index)
    {
        if (index >= 0 && index < furnitureUIs.Length)
        {
            furnitureUIs[index].SetActive(false);
        }
        else
        {
            Debug.LogError("Invalid Furniture UI index");
        }
    }

    // 控制分页功能
    public void NextPage()
    {
        if (currentFurniturePage < furnitureUIs.Length - 1)
        {
            HideFurnitureUI(currentFurniturePage);
            currentFurniturePage++;
            ShowFurnitureUI(currentFurniturePage);
        }
        else
        {
            Debug.Log("You are already on the last page.");
        }
    }

    public void PreviousPage()
    {
        if (currentFurniturePage > 0)
        {
            HideFurnitureUI(currentFurniturePage);
            currentFurniturePage--;
            ShowFurnitureUI(currentFurniturePage);
        }
        else
        {
            Debug.Log("You are already on the first page.");
        }
    }

    // 初始化显示第一个家具 UI 页面
    public void InitializeFurnitureUI()
    {
        ShowFurnitureUI(currentFurniturePage);
    }
}
