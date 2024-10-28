using UnityEngine;
using UnityEngine.UI; // Required for UI Text
using System.Collections.Generic;
using System.Collections;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab; // 石頭的Prefab
    public float scrollSpeed = 50f; // 移動速度

    [Header("Initial Positions for Rocks")]
    public Vector2[] initialPositions; // 用於設置石頭的初始位置
    public float respawnPositionX = -800f; // 石頭的重生位置 (螢幕左邊界外)
    public float disappearOffset = 100f; // 控制石頭消失的位置偏移量

    public Text startText; // UI text for start message
    public Text countdownText; // UI text for countdown display
    public Text timerText; // UI text for timer display

    private RectTransform[] rocks; // 石頭的RectTransform
    private int rockCount = 3; // 石頭的數量
    private List<int> activeRocks; // 用於追踪活躍的石頭索引
    private bool gameStarted = false; // 控制遊戲開始的布林值

    public float countdownDuration = 60f; // Countdown duration in seconds
    private float timeRemaining; // Time remaining for the countdown
    private bool isCountingDown = false; // To check if the countdown is active

    public bool IsGameStarted => gameStarted; // Exposing the gameStarted state

    void Start()
    {
        rocks = new RectTransform[rockCount];
        activeRocks = new List<int>();

        // Set the start text message
        startText.text = "Press 'Space' To Start";
        
        for (int i = 0; i < rockCount; i++)
        {
            rocks[i] = Instantiate(rockPrefab, initialPositions[i], Quaternion.identity, transform).GetComponent<RectTransform>();
            rocks[i].anchoredPosition = new Vector2(respawnPositionX, initialPositions[i].y); // Start off-screen
        }

        // Randomly select two rocks to appear initially
        UpdateActiveRocks();
        countdownText.gameObject.SetActive(false); // Hide countdown initially
        timerText.gameObject.SetActive(false); // Hide timer initially
    }

    void Update()
    {
        // Start the countdown on space bar press
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            startText.gameObject.SetActive(false); // Hide start text
            countdownText.gameObject.SetActive(true); // Show countdown
            StartCoroutine(StartCountdown()); // Start the countdown coroutine
        }

        // Only move rocks and update timer if the game has started
        if (gameStarted)
        {
            // Move active rocks to the left
            for (int i = 0; i < activeRocks.Count; i++)
            {
                int rockIndex = activeRocks[i];
                rocks[rockIndex].anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

                // Calculate disappear position
                float disappearPositionX = -Screen.width / 2 - disappearOffset;

                // Check if rock is off-screen
                if (rocks[rockIndex].anchoredPosition.x <= disappearPositionX)
                {
                    // Respawn the rock at the specified position
                    rocks[rockIndex].anchoredPosition = new Vector2(respawnPositionX, rocks[rockIndex].anchoredPosition.y);

                    // Randomly select two rocks to respawn
                    UpdateActiveRocks();
                    break;
                }
            }

            // Update timer if counting down
            if (isCountingDown)
            {
                timeRemaining -= Time.deltaTime; // Decrease time remaining
                timerText.text = FormatTime(timeRemaining); // Update timer display

                // Check if the countdown has reached zero
                if (timeRemaining <= 0)
                {
                    timeRemaining = 0; // Clamp to zero
                    isCountingDown = false; // Stop the countdown
                    timerText.text = "Time's Up!"; // Display message
                    // Optionally, you can add game over logic here
                    Debug.Log("Game Over! Time's up!");
                }
            }
        }
    }

    private void UpdateActiveRocks()
    {
        activeRocks.Clear();

        // Randomly select two indices for active rocks
        List<int> indices = new List<int> { 0, 1, 2 };
        for (int i = 0; i < 2; i++)
        {
            int randomIndex = indices[Random.Range(0, indices.Count)];
            activeRocks.Add(randomIndex);
            indices.Remove(randomIndex);

            // Set the active rock to its initial position
            rocks[randomIndex].anchoredPosition = initialPositions[randomIndex];
        }
    }

    // Coroutine for 3-second countdown
    private IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false); // Hide countdown text
        timerText.gameObject.SetActive(true); // Show timer text
        timeRemaining = countdownDuration; // Set initial time remaining
        isCountingDown = true; // Start the timer
        gameStarted = true; // Start the game
    }

    // Format time for display (MM:SS)
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}