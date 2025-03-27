using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

sing UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class UniTaskExample : MonoBehaviour
{
    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        StartLogging();
    }

    public void StartLogging()
    {
        StopLogging(); // 既に実行中なら停止（多重起動を防ぐ）
        cancellationTokenSource = new CancellationTokenSource();
        LogEverySecond(cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid LogEverySecond(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                Debug.Log("UniTask 実行中：" + Time.time);
                await UniTask.Delay(1000, cancellationToken: token); // キャンセル可能な遅延
            }
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("UniTask がキャンセルされました");
        }
        finally
        {
            Debug.Log("UniTask ループが完全に終了しました");
        }
    }

    public void StopLogging()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel(); // タスクをキャンセル
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
            Debug.Log("UniTask 停止！");
        }
    }
}