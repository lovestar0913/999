using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button startButton; // 開始遊戲按鈕
    public GameObject buttonPrefab; // 按鈕預製體
    public Transform gameArea; // 按鈕生成範圍 (UI 物件)
    public GameObject gameOverPanel; // 遊戲結束 UI (包含 "遊戲結束！" 文字和重新開始按鈕)
    public Text scoreText; // 顯示分數的 UI 文本

    private List<GameObject> spawnedButtons = new List<GameObject>(); // 存放當前的按鈕
    private int maxButtons = 10; // 最大按鈕數量
    private int score = 0; // 玩家分數
    private Coroutine spawnCoroutine; // 存儲 Coroutine

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        gameOverPanel.SetActive(false); // 隱藏遊戲結束畫面
        scoreText.text = "Score: 0"; // 初始化分數顯示
    }

    // 開始遊戲
    void StartGame()
    {
        startButton.gameObject.SetActive(false); // 隱藏「開始遊戲」按鈕
        gameOverPanel.SetActive(false); // 隱藏「遊戲結束」畫面
        score = 0; // 重置分數
        scoreText.text = "Score: " + score; // 更新分數顯示
        ClearAllButtons(); // **確保沒有舊的按鈕**
        spawnCoroutine = StartCoroutine(SpawnButtons()); // 啟動 Coroutine
    }

    // 每 3 秒生成 3 個隨機按鈕
    IEnumerator SpawnButtons()
    {
        while (true)
        {
            if (spawnedButtons.Count >= maxButtons)
            {
                StopGame(); // **按鈕超過 10，馬上停止**
                yield break;
            }

            for (int i = 0; i < 3; i++)
            {
                if (spawnedButtons.Count >= maxButtons) // **在生成新按鈕前再檢查**
                {
                    StopGame();
                    yield break;
                }

                SpawnRandomButton();
            }

            yield return new WaitForSeconds(2f); // **修正為 2 秒刷新**
        }
    }

    // 隨機生成按鈕
    void SpawnRandomButton()
    {
        if (spawnedButtons.Count >= maxButtons) return;

        RectTransform gameAreaRect = gameArea.GetComponent<RectTransform>();

        float randomX = Random.Range(-gameAreaRect.rect.width / 2, gameAreaRect.rect.width / 2);
        float randomY = Random.Range(-gameAreaRect.rect.height / 2, gameAreaRect.rect.height / 2);

        GameObject newButton = Instantiate(buttonPrefab, gameArea);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);

        newButton.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(newButton));

        spawnedButtons.Add(newButton);
    }

    // 點擊按鈕後銷毀它並增加分數
    void ButtonClicked(GameObject button)
    {
        score += 10; // 每點擊一個按鈕，得 10 分
        scoreText.text = "Score: " + score; // 更新分數顯示
        DestroyButton(button); // 銷毀按鈕
    }

    // 銷毀按鈕
    void DestroyButton(GameObject button)
    {
        spawnedButtons.Remove(button);
        Destroy(button);
    }

    // 停止遊戲
    void StopGame()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        ClearAllButtons(); // **刪除所有按鈕**
        gameOverPanel.SetActive(true); // **顯示 "遊戲結束"**

        Debug.Log("遊戲結束！按鈕數量達到 10 個");
    }

    // 刪除所有按鈕
    void ClearAllButtons()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear(); // 清空列表
    }
}