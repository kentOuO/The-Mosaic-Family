using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpamEating : MonoBehaviour
{
    public Text countdownText; // Assign your countdown Text (Legacy) in the Inspector
    public Text timerText;     // Assign your timer Text (Legacy) in the Inspector
    public Text scoreText;     // Assign your score Text (Legacy) in the Inspector
    public GameObject gameUIPanel; // Assign your UI panel in the Inspector
    public float gameDuration = 30f; // Game duration in seconds
    public int score = 0;           // Player's score

    private float timeRemaining;
    private bool isGameActive = false;

    void OnEnable() // When the game UI is enabled, reset the game state
    {
        ResetGame();
    }

    void ResetGame() // Reset game state
    {
        countdownText.gameObject.SetActive(false);
        score = 0; // Reset score
        timeRemaining = gameDuration; // Reset timer
        UpdateScoreText();
        UpdateTimerText();
        isGameActive = false; // Ensure the game is not active
        StopAllCoroutines(); // Stop all running coroutines
        StartCoroutine(ShowReadyMessage()); // Show "Ready?" message before starting the game
    }

    void Update()
    {
        if (isGameActive)
        {
            HandleInput();
            UpdateTimer();
        }
    }

    // Show "Ready?" and wait for the player to press "K"
    IEnumerator ShowReadyMessage()
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Ready?\n Press K to Start "; // Display "Ready?"
        
        // Wait until the player presses the "K" key
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.K));

        // Once "K" is pressed, start the countdown
        StartCoroutine(CountdownToStart());
    }

    // Pre-game countdown before starting the actual game
    IEnumerator CountdownToStart()
    {
        // Countdown logic
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f); // Wait 1 second between each number
        }

        countdownText.text = "Go!"; // Display "Go!" at the end
        yield return new WaitForSeconds(1f);    // Wait another second

        // Hide the countdown text and start the game
        countdownText.gameObject.SetActive(false);
        StartGame(); // Start the game timer
    }

    void StartGame()
    {
        isGameActive = true;
        timeRemaining = gameDuration; // Reset timer
        score = 0; // Reset score
        UpdateScoreText();
        UpdateTimerText();
        StartCoroutine(SpawnItems());
    }

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        UpdateTimerText();

        if (timeRemaining <= 0)
        {
            EndGame();
        }
    }

    void HandleInput()
    {
        // Use J and L keys to "eat"
        if (isGameActive && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.L)))
        {
            EatItem();
        }
    }

    void EatItem()
    {
        if (isGameActive) // Ensure score is only added when the game is active
        {
            score++; // Increment score
            UpdateScoreText(); // Update score display
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void UpdateTimerText()
    {
        timerText.text = "Time: " + Mathf.Max(0, Mathf.RoundToInt(timeRemaining)).ToString();
    }

    void EndGame()
    {
        isGameActive = false;
        StopAllCoroutines();

        if (score > 300)
        {
            // Game finished successfully, close the UI
            countdownText.gameObject.SetActive(true);
            countdownText.text = "You Win!";
            StartCoroutine(CloseGameUI()); // Close the game UI
        }
        else
        {
            // Player scored less than 250, display "I am too hungry" and allow replay
            countdownText.gameObject.SetActive(true);
            countdownText.text = "I am too hungry! \nI want to eat more!";
            StartCoroutine(CountdownToRetry());
        }
    }

    // Coroutine to close the UI after a delay (you can implement actual UI close functionality here)
    IEnumerator CloseGameUI()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before closing UI
        gameUIPanel.SetActive(false); // Hide the game UI by deactivating it
        Debug.Log("Closing game UI...");
    }

    IEnumerator CountdownToRetry()
    {
        // Wait for the player to press "K"
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.K));

        // Countdown logic
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString(); // Update the countdown text
            yield return new WaitForSeconds(1f); // Wait for 1 second
        }

        // After countdown, hide the countdown text and restart the game
        countdownText.gameObject.SetActive(false);
        StartGame(); // Restart the game
    }

    IEnumerator SpawnItems()
    {
        while (isGameActive)
        {
            // Spawn items or perform any other action here if needed
            yield return new WaitForSeconds(1f); // Adjust the interval as needed
        }
    }
}
