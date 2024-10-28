using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public RectTransform background1; // 第一張背景圖片
    public RectTransform background2; // 第二張背景圖片
    public float scrollSpeed = 50f; // 滾動速度
    public RockSpawner rockSpawner; // Reference to RockSpawner

    private float backgroundWidth;

    void Start()
    {
        // 假設兩張背景圖片的寬度相同
        backgroundWidth = background1.rect.width;

        // 自動設置背景圖片的位置，使它們無縫拼接
        background2.anchoredPosition = new Vector2(background1.anchoredPosition.x + backgroundWidth, background1.anchoredPosition.y);
    }

    void Update()
    {
        // Check if the game has started before scrolling
        if (rockSpawner != null && rockSpawner.IsGameStarted)
        {
            // Move backgrounds to the left
            background1.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;
            background2.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            // Move background1 back to the right if it goes off-screen
            if (background1.anchoredPosition.x <= -backgroundWidth)
            {
                background1.anchoredPosition += new Vector2(backgroundWidth * 2, 0);
            }

            // Move background2 back to the right if it goes off-screen
            if (background2.anchoredPosition.x <= -backgroundWidth)
            {
                background2.anchoredPosition += new Vector2(backgroundWidth * 2, 0);
            }
        }
    }
}
