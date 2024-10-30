using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class RockSpawner : MonoBehaviour
{
    public List<GameObject> rockPrefabs; // 用於存儲不同的 prefab
    public float scrollSpeed = 50f;

    [Header("Initial Positions for Rocks")]
    public Vector2[] initialPositions;
    public float respawnPositionX = -800f;
    public float disappearOffset = 100f;
    public PlayerMovementInImage playerMovement;

    public Text startText;
    public Text countdownText;
    public Text timerText;
    public Text warningText;

    private RectTransform[] rocks;
    private int rockCount = 3;
    private List<int> activeRocks;
    private bool gameStarted = false;

    public float countdownDuration = 60f;
    private float timeRemaining;
    private bool isCountingDown = false;

    // Difficulty scaling variables
    public float speedIncreaseInterval = 10f; // Interval in seconds for speed increase
    public float speedIncreaseAmount = 5f; // Amount to increase speed each interval
    private float nextSpeedIncreaseTime;

    // Other settings
    public RectTransform[] targetImages; // Target objects to avoid

    public bool IsGameStarted => gameStarted;

    void Start()
    {
        rocks = new RectTransform[rockCount];
        activeRocks = new List<int>();

        startText.text = "Press 'Space' To Start";
        warningText.gameObject.SetActive(false);

        for (int i = 0; i < rockCount; i++)
        {
            // 隨機選擇一個 prefab
            GameObject randomRockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Count)];
            rocks[i] = Instantiate(randomRockPrefab, initialPositions[i], Quaternion.identity, transform).GetComponent<RectTransform>();
            rocks[i].anchoredPosition = new Vector2(respawnPositionX, initialPositions[i].y);
            rocks[i].SetSiblingIndex(transform.childCount - 2); // Set layer position
        }

        UpdateActiveRocks();
        countdownText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        // Set the first speed increase time
        nextSpeedIncreaseTime = Time.time + speedIncreaseInterval;
    }

    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (gameStarted)
        {
            UpdateRocks();
            UpdateTimer();
            IncreaseRockSpeedOverTime();
        }
    }

    private void StartGame()
    {
        startText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    private void UpdateRocks()
    {
        for (int i = 0; i < activeRocks.Count; i++)
        {
            int rockIndex = activeRocks[i];
            rocks[rockIndex].anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            foreach (RectTransform targetImage in targetImages)
            {
                if (IsOverlapping(rocks[rockIndex], targetImage))
                {
                    ShowWarning();
                    RestartGame();
                    return;
                }
            }

            float disappearPositionX = -Screen.width / 2 - disappearOffset;
            if (rocks[rockIndex].anchoredPosition.x <= disappearPositionX)
            {
                rocks[rockIndex].anchoredPosition = new Vector2(respawnPositionX, rocks[rockIndex].anchoredPosition.y);
                UpdateActiveRocks();
                break;
            }
        }
    }

    private void UpdateTimer()
    {
        if (isCountingDown)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = FormatTime(timeRemaining);

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isCountingDown = false;
                timerText.text = "Time's Up!";
                Debug.Log("Game Over! Time's up!");
                RestartGame();
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        timeRemaining = countdownDuration;
        isCountingDown = true;
        gameStarted = true;

        if (playerMovement != null)
        {
            playerMovement.SetRunAnimation(true); // Start running animation
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect rect1World = RectTransformToScreenSpace(rect1);
        Rect rect2World = RectTransformToScreenSpace(rect2);
        return rect1World.Overlaps(rect2World);
    }

    private Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector3[] corners = new Vector3[4];
        transform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }

    private void ShowWarning()
    {
        warningText.gameObject.SetActive(true);
        Invoke("HideWarning", 2f); // Hide after 2 seconds
    }

    private void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        gameStarted = false;
        startText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        isCountingDown = false;
        timeRemaining = countdownDuration;

        if (playerMovement != null)
        {
            playerMovement.SetRunAnimation(false);
        }

        foreach (var rock in rocks)
        {
            rock.anchoredPosition = new Vector2(respawnPositionX, rock.anchoredPosition.y);
        }
        UpdateActiveRocks();
    }

    private void UpdateActiveRocks()
    {
        activeRocks.Clear();
        List<int> indices = new List<int> { 0, 1, 2 };
        for (int i = 0; i < 2; i++)
        {
            int randomIndex = indices[Random.Range(0, indices.Count)];
            activeRocks.Add(randomIndex);
            indices.Remove(randomIndex);

            rocks[randomIndex].anchoredPosition = initialPositions[randomIndex];
        }
    }

    private void IncreaseRockSpeedOverTime()
    {
        if (Time.time >= nextSpeedIncreaseTime)
        {
            scrollSpeed += speedIncreaseAmount; // Increase scroll speed
            nextSpeedIncreaseTime = Time.time + speedIncreaseInterval; // Set time for next increase
        }
    }
}
