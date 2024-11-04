using UnityEngine;
using UnityEngine.UI;

public class AnxietyTest : MonoBehaviour
{
    public RectTransform marker; // 移動標記
    public RectTransform bar;    // bar
    public RectTransform redZone; // 紅色區域
    public float speed = 200f;    // 初始移動速度
    public float speedIncrease = 50f; // 每次成功按下後的速度增加量
    private bool movingRight = true; // 標記的移動方向
    private bool stopped = false;    // 是否已經停止

    public int requiredPresses = 5; // 玩家需要按下 "F" 的次數
    private int currentPresses = 0;  // 當前成功按下 "F" 的次數
    public float allowedTime = 10f;  // 玩家必須在這個時間內按下5次
    private float timeLeft;          // 剩餘的時間

    public float redZoneWidthDecrease = 10f; // 每次成功按下後減少的紅色區域寬度

    void OnEnable()
    {
        ResetGame(); // 在開始時重置遊戲
    }

    void Update()
    {
        if (!stopped)
        {
            MoveMarker();
        }

        // 玩家按下 F 鍵
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stopped)
            {
                ResetGame(); // 如果遊戲已經停止，按F重新開始
            }
            else
            {
                TryPress();  // 如果遊戲還在進行中，檢查按F是否在紅色區域內
            }
        }

        // 倒數計時
        if (!stopped)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                LoseGame();
            }
        }
    }

    // 隨機放置紅色區域
    void PositionRedZoneRandomly()
    {
        float barWidth = bar.rect.width;
        float redZoneWidth = redZone.rect.width;
        float minPositionX = -barWidth / 2 + redZoneWidth / 2;
        float maxPositionX = barWidth / 2 - redZoneWidth / 2;
        float randomX = Random.Range(minPositionX, maxPositionX);
        redZone.anchoredPosition = new Vector2(randomX, redZone.anchoredPosition.y);
    }

    void MoveMarker()
    {
        float step = speed * Time.deltaTime;
        if (movingRight)
        {
            marker.anchoredPosition += new Vector2(step, 0);
            if (marker.anchoredPosition.x >= bar.rect.width / 2) // 到達bar的右邊
            {
                movingRight = false;
            }
        }
        else
        {
            marker.anchoredPosition -= new Vector2(step, 0);
            if (marker.anchoredPosition.x <= -bar.rect.width / 2) // 到達bar的左邊
            {
                movingRight = true;
            }
        }
    }

    // 玩家嘗試按下 "F"
    void TryPress()
    {
        if (IsMarkerInRedZone())
        {
            currentPresses++;
            Debug.Log("Success! Pressed in the red zone. Current presses: " + currentPresses);

            // 增加移動速度
            speed += speedIncrease;
            Debug.Log("Marker speed increased to: " + speed);

            // 縮小紅色區域的寬度
            RectTransform redZoneRect = redZone.GetComponent<RectTransform>();
            Vector2 newSize = redZoneRect.sizeDelta;
            newSize.x = Mathf.Max(0, newSize.x - redZoneWidthDecrease); // 確保不會縮小到負值
            redZoneRect.sizeDelta = newSize;

            if (currentPresses >= requiredPresses)
            {
                WinGame();
            }
            else
            {
                PositionRedZoneRandomly(); // 成功按下後，紅色區域移動到新位置
            }
        }
        else
        {
            Debug.Log("Failed! Wrong click, you lose the game.");
            LoseGame(); // 玩家按錯，直接結束遊戲
        }
    }

    // 檢查標記是否在紅色區域內
    bool IsMarkerInRedZone()
    {
        return marker.anchoredPosition.x >= redZone.anchoredPosition.x - redZone.rect.width / 2 &&
               marker.anchoredPosition.x <= redZone.anchoredPosition.x + redZone.rect.width / 2;
    }

    // 通關邏輯
    void WinGame()
    {
        stopped = true;
        Debug.Log("Congratulations! You won the game by pressing 'F' in the red zone 5 times!");
        // 在這裡添加通關邏輯
    }

    // 失敗邏輯
    void LoseGame()
    {
        stopped = true;
        Debug.Log("You lost the game. Press 'F' to restart.");
        // 玩家失敗後，提示玩家按F重新開始
    }

    // 重置遊戲
    void ResetGame()
    {
        stopped = false;
        currentPresses = 0;
        timeLeft = allowedTime;

        // 重置標記位置
        marker.anchoredPosition = new Vector2(-bar.rect.width / 2, marker.anchoredPosition.y); // 可以根據需要更改起始位置

        // 重置紅色區域
        RectTransform redZoneRect = redZone.GetComponent<RectTransform>();
        redZoneRect.sizeDelta = new Vector2(100, redZoneRect.sizeDelta.y); // 設置初始寬度（根據需要調整）
        PositionRedZoneRandomly(); // 隨機放置紅色區域

        // 重置速度
        speed = 200f;
        Debug.Log("Game restarted.");
    }
}
