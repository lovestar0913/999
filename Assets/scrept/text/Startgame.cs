using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    // 该函数将在按钮被点击时调用
    public void StartGame()
    {
    // 销毁按钮自身
        Destroy(gameObject);
    }
}