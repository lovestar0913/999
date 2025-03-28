using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button startButton; // �}�l�C�����s
    public GameObject buttonPrefab; // ���s�w�s��
    public Transform gameArea; // ���s�ͦ��d�� (UI ����)

    private List<GameObject> spawnedButtons = new List<GameObject>(); // �s���e�����s

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    // �}�l�C���ɡA���ë��s�ö}�l�`���ͦ����s
    void StartGame()
    {
        startButton.gameObject.SetActive(false); // ���áu�}�l�C���v���s
        StartCoroutine(SpawnButtons()); // �Ұ� Coroutine�A�C 3 ���s���s
    }

    // �C 3 ��ͦ� 3 ���H�����s
    IEnumerator SpawnButtons()
    {
        while (true)
        {
            ClearOldButtons(); // �M���ª����s

            for (int i = 0; i < 3; i++)
            {
                SpawnRandomButton(); // �ͦ��s���s
            }

            yield return new WaitForSeconds(2f); // ���� 3 ��A�ͦ��s���@�ի��s
        }
    }

    // �M���ª����s
    void ClearOldButtons()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear(); // �M�ŦC��
    }

    // �H���ͦ����s
    void SpawnRandomButton()
    {
        // ���o�C���ϰ쪺�d��
        RectTransform gameAreaRect = gameArea.GetComponent<RectTransform>();

        // �H���y��
        float randomX = Random.Range(-gameAreaRect.rect.width / 2, gameAreaRect.rect.width / 2);
        float randomY = Random.Range(-gameAreaRect.rect.height / 2, gameAreaRect.rect.height / 2);

        // �Ыث��s
        GameObject newButton = Instantiate(buttonPrefab, gameArea);
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);

        // �K�[�I���ƥ�
        newButton.GetComponent<Button>().onClick.AddListener(() => DestroyButton(newButton));

        // �[�J�C��A��K����R��
        spawnedButtons.Add(newButton);
    }

    // �I�����s��P����
    void DestroyButton(GameObject button)
    {
        spawnedButtons.Remove(button);
        Destroy(button);
    }
}