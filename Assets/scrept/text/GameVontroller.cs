using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button startButton; // 開始遊戲按鈕
    public GameObject buttonPrefab; // 按鈕預製體
    public Transform gameArea; // 按鈕生成範圍 (UI 物件)

    private List<GameObject> spawnedButtons = new List<GameObject>(); // 存放當前的按鈕

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    // 開始遊戲時，隱藏按鈕並開始循環生成按鈕
    void StartGame()
    {
        startButton.gameObject.SetActive(false); // 隱藏「開始遊戲」按鈕
        StartCoroutine(SpawnButtons()); // 啟動 Coroutine，每 3 秒刷新按鈕
    }

    // 每 3 秒生成 3 個隨機按鈕
    IEnumerator SpawnButtons()
    {
        while (true)
        {
            ClearOldButtons(); // 清除舊的按鈕

            for (int i = 0; i < 3; i++)
            {
                SpawnRandomButton(); // 生成新按鈕
            }

            yield return new WaitForSeconds(2f); // 等待 3 秒再生成新的一組按鈕
        }
    }

    // 清除舊的按鈕
    void ClearOldButtons()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear(); // 清空列表
    }

    // 隨機生成按鈕
    void SpawnRandomButton()
    {
        // 取得遊戲區域的範圍
        RectTransform gameAreaRect = gameArea.GetComponent<RectTransform>();

        // 隨機座標
        float randomX = Random.Range(-gameAreaRect.rect.width / 2, gameAreaRect.rect.width / 2);
        float randomY = Random.Range(-gameAreaRect.rect.height / 2, gameAreaRect.rect.height / 2);

        // 創建按鈕
        GameObject newButton = Instantiate(buttonPrefab, gameArea);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);

        // 添加點擊事件
        newButton.GetComponent<Button>().onClick.AddListener(() => DestroyButton(newButton));

        // 加入列表，方便之後刪除
        spawnedButtons.Add(newButton);
    }

    // 點擊按鈕後銷毀它
    void DestroyButton(GameObject button)
    {
        spawnedButtons.Remove(button);
        Destroy(button);
    }
}