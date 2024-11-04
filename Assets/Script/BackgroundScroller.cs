using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public RectTransform background1; // First background image
    public RectTransform background2; // Second background image
    public float scrollSpeed = 50f; // Initial scroll speed
    public RockSpawner rockSpawner; // Reference to RockSpawner

    private float backgroundWidth;
    private float speedIncreaseInterval = 5f; // Increase scroll speed every 5 seconds
    private float timeSinceLastIncrease = 0f;

    void Start()
    {
        // Assume both background images have the same width
        backgroundWidth = background1.rect.width;

        // Automatically position the background images for seamless scrolling
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

            // Increase scroll speed every 5 seconds only if the game has started
            timeSinceLastIncrease += Time.deltaTime;
            if (timeSinceLastIncrease >= speedIncreaseInterval)
            {
                scrollSpeed += 50f; // Increase scroll speed
                timeSinceLastIncrease = 0f; // Reset the timer
            }
        }
    }
}
