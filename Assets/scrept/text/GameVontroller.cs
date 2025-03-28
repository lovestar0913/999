using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button startButton; // �}�l�C�����s
    public GameObject buttonPrefab; // ���s�w�s��
    public Transform gameArea; // ���s�ͦ��d�� (UI ����)
    public GameObject gameOverPanel; // �C������ UI (�]�t "�C�������I" ��r�M���s�}�l���s)
    public Text scoreText; // ��ܤ��ƪ� UI �奻

    private List<GameObject> spawnedButtons = new List<GameObject>(); // �s���e�����s
    private int maxButtons = 10; // �̤j���s�ƶq
    private int score = 0; // ���a����
    private Coroutine spawnCoroutine; // �s�x Coroutine

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        gameOverPanel.SetActive(false); // ���ùC�������e��
        scoreText.text = "Score: 0"; // ��l�Ƥ������
    }

    // �}�l�C��
    void StartGame()
    {
        startButton.gameObject.SetActive(false); // ���áu�}�l�C���v���s
        gameOverPanel.SetActive(false); // ���áu�C�������v�e��
        score = 0; // ���m����
        scoreText.text = "Score: " + score; // ��s�������
        ClearAllButtons(); // **�T�O�S���ª����s**
        spawnCoroutine = StartCoroutine(SpawnButtons()); // �Ұ� Coroutine
    }

    // �C 3 ��ͦ� 3 ���H�����s
    IEnumerator SpawnButtons()
    {
        while (true)
        {
            if (spawnedButtons.Count >= maxButtons)
            {
                StopGame(); // **���s�W�L 10�A���W����**
                yield break;
            }

            for (int i = 0; i < 3; i++)
            {
                if (spawnedButtons.Count >= maxButtons) // **�b�ͦ��s���s�e�A�ˬd**
                {
                    StopGame();
                    yield break;
                }

                SpawnRandomButton();
            }

            yield return new WaitForSeconds(2f); // **�ץ��� 2 ���s**
        }
    }

    // �H���ͦ����s
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

    // �I�����s��P�����üW�[����
    void ButtonClicked(GameObject button)
    {
        score += 10; // �C�I���@�ӫ��s�A�o 10 ��
        scoreText.text = "Score: " + score; // ��s�������
        DestroyButton(button); // �P�����s
    }

    // �P�����s
    void DestroyButton(GameObject button)
    {
        spawnedButtons.Remove(button);
        Destroy(button);
    }

    // ����C��
    void StopGame()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        ClearAllButtons(); // **�R���Ҧ����s**
        gameOverPanel.SetActive(true); // **��� "�C������"**

        Debug.Log("�C�������I���s�ƶq�F�� 10 ��");
    }

    // �R���Ҧ����s
    void ClearAllButtons()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear(); // �M�ŦC��
    }
}