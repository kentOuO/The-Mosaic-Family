using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public RectTransform background1;
    public RectTransform background2;
    public float scrollSpeed = 50f;
    public float preGameScrollSpeed = 20f;
    public RockSpawner rockSpawner;

    private float backgroundWidth;

    void Start()
    {
        backgroundWidth = background1.rect.width;
        background2.anchoredPosition = new Vector2(background1.anchoredPosition.x + backgroundWidth, background1.anchoredPosition.y);
    }

    void Update()
    {
        if (rockSpawner != null && rockSpawner.IsGameStarted)
        {
            scrollSpeed = rockSpawner.scrollSpeed; // Sync with RockSpawner

            background1.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;
            background2.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            if (background1.anchoredPosition.x <= -backgroundWidth)
            {
                background1.anchoredPosition += new Vector2(backgroundWidth * 2, 0);
            }

            if (background2.anchoredPosition.x <= -backgroundWidth)
            {
                background2.anchoredPosition += new Vector2(backgroundWidth * 2, 0);
            }
        }
        else
        {
            background1.anchoredPosition += Vector2.left * preGameScrollSpeed * Time.deltaTime;
            background2.anchoredPosition += Vector2.left * preGameScrollSpeed * Time.deltaTime;

            if (background1.anchoredPosition.x <= -backgroundWidth)
            {
                background1.anchoredPosition += new Vector2(backgroundWidth * 2, 0);
            }

            if (background2.anchoredPosition.x <= -backgroundWidth)
            {
                background2.anchoredPosition += new Vector2(backgroundWidth * 2, 0);
            }
        }
    }
}
